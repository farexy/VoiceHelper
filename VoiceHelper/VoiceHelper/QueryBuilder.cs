using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        private bool _ordered;
        
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

                if (i != tokens.Count - 1)
                {
                    if (tokens[i].Type == TokenType.SortBy || tokens[i].Type == TokenType.SortByNext)
                    {
                        var text = "";
                        Token token = null;
                        while (i < tokens.Count - 1 && tokens[i + 1].Type is TokenType.Text)
                        {
                            token = tokens[++i];
                            text += token.Value;
                        }

                        query = ResolveOrderByText(text, query, token?.Type is TokenType.SortDesc);

                    }
                    
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
        
        
        
        private IQueryable<Product> ResolveOrderByText(string orderingText, IQueryable<Product> query, bool desc)
        {
            if (orderingText.Equals("price", StringComparison.InvariantCultureIgnoreCase))
            {
                if (_ordered)
                {
                    query = desc 
                        ? (query as IOrderedQueryable<Product>).ThenByDescending(p => p.Price)
                        : (query as IOrderedQueryable<Product>).ThenBy(p => p.Price);
                }
                else
                {
                    query = desc ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price);
                }
                
            }
            if (orderingText.Equals("category", StringComparison.InvariantCultureIgnoreCase))
            {
                if (_ordered)
                {
                    query = desc 
                        ? (query as IOrderedQueryable<Product>).ThenByDescending(p => p.Category)
                        : (query as IOrderedQueryable<Product>).ThenBy(p => p.Category);
                }
                else
                {
                    query = desc ? query.OrderByDescending(p => p.Category) : query.OrderBy(p => p.Category);
                }
            }
            if (orderingText.Equals("name", StringComparison.InvariantCultureIgnoreCase))
            {
                if (_ordered)
                {
                    query = desc 
                        ? (query as IOrderedQueryable<Product>).ThenByDescending(p => p.Name)
                        : (query as IOrderedQueryable<Product>).ThenBy(p => p.Name);
                }
                else
                {
                    query = desc ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name);
                }
            }

            _ordered = true;
            return query;
        }
    }
}