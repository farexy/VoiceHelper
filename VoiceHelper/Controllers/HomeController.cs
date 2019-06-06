using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using VoiceHelper.Db;
using VoiceHelper.Models;
using VoiceHelper.VoiceHelper;

namespace VoiceHelper.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly SpeechToTextConverter _speechToTextConverter;

        public HomeController(IHostingEnvironment hostingEnvironment, 
            ApplicationContext context, 
            SpeechToTextConverter speechToTextConverter)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
            _speechToTextConverter = speechToTextConverter;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.ToListAsync();
            return View(new ProductsViewModel{Products = products});
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload()
        {
            var file = Request.Form.Files.First();
            var fileName = "file.m4a";

            var filePath = _hostingEnvironment.WebRootPath + '\\' + fileName;
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            var res = await _speechToTextConverter.ConvertAsync(filePath);
            var tokens = new SpeechParser().Parse(res.Substring(0, res.Length - 1));
            var records = await new QueryBuilder(_context.Products).BuildQuery(tokens).ToListAsync();
            return View("Index", new ProductsViewModel{Products = records});
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
