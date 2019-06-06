using System;
using System.Collections.Generic;
using System.Linq;
using VoiceHelper.Db;

namespace VoiceHelper.VoiceHelper
{
    public class QueryBuilder
    {
        private readonly IQueryable<Product> _dbCollection;
        private bool? _orderByContext;
        private bool? _findContext;
        private bool? _categoryContext;

        private string _nameFindRequest;
        private string _categoryFindRequest;

        private int? _nameOrder;
        private int? _categoryOrder;
        private int? _priceOrder;
        
        public QueryBuilder(IQueryable<Product> dbCollection)
        {
            _dbCollection = dbCollection;
        }
//
//        public void Set(List<Token> tokens)
//        {
//            
//            switch (token.Type)
//            {
//                case TokenType.SortBy when _orderByContext.HasValue:
//                    throw new SpeechParsingException("Order by is already opened");
//                case TokenType.SortBy:
//                    _orderByContext = true;
//                    _findContext = false;
//                    break;
//                case TokenType.SortByNext when !_orderByContext.HasValue || !_orderByContext.Value:
//                    throw new SpeechParsingException("");
//                case TokenType.FindRequest when _findContext.HasValue:
//                    throw new SpeechParsingException("Find is already opened");
//                case TokenType.FindRequest:
//                    _findContext = true;
//                    _orderByContext = false;
//                    break;
//                case TokenType.Category:
//                    _categoryContext = true;
//                    _orderByContext = false;
//                    break;
//            }
//        }

        public IQueryable<Product> BuildQuery(List<Token> tokens)
        {
            IQueryable<Product> query = _dbCollection;
            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].Type is TokenType.SortBy)
                {
                    
                }

                if (tokens[i].Type is TokenType.FindRequest)
                {
                    var text = "";
                    var token = tokens[++i];
                    while (token.Type is TokenType.Text)
                    {
                        text += token.Value;
                        if (i == tokens.Count - 1)
                        {
                            break;
                        }
                        token = tokens[++i];
                    }

                    query = query.Where(p => p.Name.Equals(text, StringComparison.InvariantCultureIgnoreCase) 
                                             || p.Category.Equals(text, StringComparison.InvariantCultureIgnoreCase));
                }
            }

            return query;
//            var query = _dbCollection;
//            if (_priceOrder.HasValue && _priceOrder > _categoryOrder && _priceOrder > _nameOrder)
//            {
//                query = query.OrderBy(p => p.Price);
//            }
//
//            if (_priceOrder.HasValue && _priceOrder < 0)
//            {
//                query = query.OrderByDescending(p => p.Price);
//            }
//
//            if (_priceOrder.HasValue && _priceOrder > _categoryOrder && _priceOrder > _nameOrder)
//            {
//                query = query.OrderBy(p => p.Price);
//            }
//
//            if (_priceOrder.HasValue && _priceOrder < 0)
//            {
//                query = query.OrderByDescending(p => p.Price);
//            }
        }
        
        private static void ResolveOrderByTokens(Token token)
        {
            
        }
    }
}