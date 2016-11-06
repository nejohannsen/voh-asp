using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using voh_asp.Services;
using voh_asp.Data;
using Microsoft.AspNetCore.Http;
using voh_asp.Models;

namespace voh_asp.Controllers
{
    public class AppFileController : Controller
    {
        private ApplicationDbContext _db { get; set; }
        private readonly IFileUpload _fileUpload;

        public AppFileController(ApplicationDbContext db, IFileUpload fileUpload)
        {
            _db = db;
            _fileUpload = fileUpload;
        }

        public ActionResult Index()
        {
            List<AppFile> dbFiles = _db.AppFiles.ToList();
            return View(dbFiles);
        }

        public ActionResult DeleteFile(string Id)
        {
            AppFile dbFile = _db.AppFiles.FirstOrDefault(t => t.AppFileId == Guid.Parse(Id));

            if (dbFile != null)
            {
                var result = _fileUpload.DeleteFile(dbFile);

                if(result)
                {
                    _db.Remove(dbFile);
                    _db.SaveChanges();
                }
            }
            return RedirectToAction("index");
        }

        public ActionResult ReplaceFile(string Id)
        {
            AppFile dbFile = _db.AppFiles.FirstOrDefault(t => t.AppFileId == Guid.Parse(Id));

            if (dbFile != null)
            {
                FileReplaceViewModel model = new FileReplaceViewModel()
                {
                    dbFile = dbFile
                };
                return View(model);
            } else
            {
                return RedirectToAction("index");
            }

        }

        [HttpPost]
        public ActionResult ReplaceFile(FileReplaceViewModel model)
        {
            AppFile dbFile = _db.AppFiles.FirstOrDefault(t => t.AppFileId == model.dbFile.AppFileId);

            if (dbFile != null)
            {
                var fileStream = model.localFile.OpenReadStream();
                var result = _fileUpload.UploadFile(fileStream, dbFile);

            }
            return RedirectToAction("index");
        }

        public ActionResult UploadFile()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> UploadFile(IFormFile file)
        {
            //Create object that hold file info
            AppFile dbFile = new AppFile()
            {
                AWSBucket = "voh-dev",
                Path = "AppFile",
                FileName = file.FileName

            };

            //Create a context transaction
            using (var dbContextTransaction = _db.Database.BeginTransaction())
            {
                //Save object to database
                //This get me access to the guid.
                _db.AppFiles.Add(dbFile);
                _db.SaveChanges();

                //Attempt to upload the file.
                var fileStream = file.OpenReadStream();
                var result = _fileUpload.UploadFile(fileStream, dbFile);

                if (result)
                {
                    //If upload is succesfull then commit the transaction and return the url
                    dbContextTransaction.Commit();
                }
                else
                {
                    //If the upload failed then rollback the transaction and return Failed message
                    dbContextTransaction.Rollback();
                    
                }
                return RedirectToAction("index");
            }
        }

    }
}