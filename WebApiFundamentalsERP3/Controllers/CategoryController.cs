using System;
using System.Linq;
using System.Web.Http;
using WebApiFundamentalsERP3.Models;
using WebApiFundamentalsERP3.Models.ViewModels;

namespace WebApiFundamentalsERP3.Controllers
{
    public class CategoryController : ApiController
    {
        // GET: api/Category
        public ResponseData GetAll()
        {
            try
            {
                NorthwindEntities db = new NorthwindEntities();
                var sonuc = db.Categories.Select(x => new
                {
                    x.CategoryID,
                    x.CategoryName,
                    x.Description
                }).ToList();
                return new ResponseData
                {
                    Success = true,
                    Data = sonuc
                };
            }
            catch (Exception ex)
            {
                return new ResponseData
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        // GET: api/Category/5
        public ResponseData GetById(int id)
        {
            try
            {
                NorthwindEntities db = new NorthwindEntities();
                var model = db.Categories.Find(id);
                if (model == null)
                    throw new Exception($"{id} id'li kategori bulunamadı");
                return new ResponseData
                {
                    Data = new
                    {
                        model.CategoryID,
                        model.CategoryName,
                        model.Description,
                        ProductCount = model.Products.Count
                    },
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new ResponseData
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        // POST: api/Category
        [HttpPost]
        public ResponseData Insert([FromBody]CategoryViewModel model)
        {
            if (model == null)
            {
                return new ResponseData
                {
                    Success = false,
                    Message = "model null olamaz"
                };
            }
            try
            {
                NorthwindEntities db = new NorthwindEntities();
                db.Categories.Add(new Category
                {
                    CategoryName = model.categoryName,
                    Description = model.description
                });
                db.SaveChanges();
                return new ResponseData
                {
                    Success = true,
                    Message = $"{model.categoryName} isimli kategori başarıyla eklenmiştir"
                };
            }
            catch (Exception ex)
            {
                return new ResponseData
                {
                    Success = false,
                    Message = "Ekleme işleminde bir hata oluştu! " + ex.Message
                };
            }
        }

        // PUT: api/Category/5
        [HttpPost]
        public ResponseData Update([FromBody]CategoryViewModel model)
        {
            if (model == null)
            {
                return new ResponseData
                {
                    Success = false,
                    Message = "model null olamaz"
                };
            }

            try
            {
                NorthwindEntities db = new NorthwindEntities();
                var guncellenecek = db.Categories.Find(model.categoryId);
                if (guncellenecek == null)
                {
                    return new ResponseData
                    {
                        Success = false,
                        Message = "Güncellenecek kayıt bulunamadı"
                    };
                }
                guncellenecek.CategoryName = model.categoryName;
                guncellenecek.Description = model.description;
                db.SaveChanges();
                return new ResponseData
                {
                    Success = true,
                    Message = $"{model.categoryName} isimli kategori başarıyla güncellenmiştir"
                };
            }
            catch (Exception ex)
            {
                return new ResponseData
                {
                    Success = false,
                    Message = $"Güncelleme işleminde bir hata oluştu {ex.Message}"
                };
            }
        }

        // DELETE: api/Category/5
        [HttpPost]
        public ResponseData Delete(int id)
        {
            try
            {
                NorthwindEntities db = new NorthwindEntities();
                var silinecek = db.Categories.Find(id);
                if (silinecek == null)
                    throw new Exception($"{id} id'li kayıt bulunamadı");
                db.Categories.Remove(silinecek);
                db.SaveChanges();
                return new ResponseData()
                {
                    Success = true,
                    Message = $"{silinecek.CategoryName} isimli kategori silinmiştir"
                };
            }
            catch (Exception ex)
            {
                return new ResponseData()
                {
                    Success = false,
                    Message = $"Kategori silme işleminde bir hata oluştu {ex.Message}"
                };
            }
        }
    }
}
