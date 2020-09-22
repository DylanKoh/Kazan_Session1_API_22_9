using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Kazan_Session1_API_22_9;

namespace Kazan_Session1_API_22_9.Controllers
{
    public class AssetsController : Controller
    {
        private Session1Entities db = new Session1Entities();

        public AssetsController()
        {
            db.Configuration.LazyLoadingEnabled = false;
        }

        // POST: Assets
        [HttpPost]
        public ActionResult Index()
        {
            var assets = db.Assets;
            return Json(assets.ToList());
        }

        // POST: Assets/Details/5
        [HttpPost]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asset asset = db.Assets.Find(id);
            if (asset == null)
            {
                return HttpNotFound();
            }
            return Json(asset);
        }

        // POST: Assets/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "ID,AssetSN,AssetName,DepartmentLocationID,EmployeeID,AssetGroupID,Description,WarrantyDate")] Asset asset)
        {
            if (ModelState.IsValid)
            {
                var findAssetName = (from x in db.Assets
                                     where x.AssetName == asset.AssetName && x.DepartmentLocationID == asset.DepartmentLocationID
                                     select x).FirstOrDefault();
                if (findAssetName != null)
                {
                    return Json("Asset already exist in location!");
                }
                else
                {
                    db.Assets.Add(asset);
                    db.SaveChanges();
                    return Json("Successfully created Asset!");
                }
                
            }

            return View(asset);
        }

        // POST: Assets/Edit/5
        [HttpPost]
        public ActionResult Edit([Bind(Include = "ID,AssetSN,AssetName,DepartmentLocationID,EmployeeID,AssetGroupID,Description,WarrantyDate")] Asset asset)
        {
            if (ModelState.IsValid)
            {
                db.Entry(asset).State = EntityState.Modified;
                db.SaveChanges();
                return Json("Successfully edited Asset!");
            }
            return View(asset);
        }

        // POST: Assets/GetCustomView
        [HttpPost]
        public ActionResult GetCustomView()
        {
            var getAssets = (from x in db.Assets
                             join y in db.DepartmentLocations on x.DepartmentLocationID equals y.ID
                             join z in db.Departments on y.DepartmentID equals z.ID
                             join a in db.AssetGroups on x.AssetGroupID equals a.ID
                             select new
                             {
                                 AssetID = x.ID,
                                 AssetName = x.AssetName,
                                 AssetGroup = a.Name,
                                 DepartmentName = z.Name,
                                 AssetSN = x.AssetSN,
                                 WarrantyDate = x.WarrantyDate
                             });
            return Json(getAssets.ToList());
        }

        // POST: Assets/GetAllSN
        [HttpPost]
        public ActionResult GetAllSN()
        {
            var getAllSN = new List<string>();
            getAllSN = (from x in db.Assets
                        select x.AssetSN).ToList();
            getAllSN.AddRange((from x in db.AssetTransferLogs
                               select x.FromAssetSN).ToList());
            getAllSN.AddRange((from x in db.AssetTransferLogs
                               select x.ToAssetSN).ToList());
            return Json(getAllSN.Distinct());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
