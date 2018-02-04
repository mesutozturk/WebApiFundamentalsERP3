using System;
using System.Linq;
using System.Web.Hosting;
using System.Web.Http;
using WebApiFundamentalsERP3.Models;
using WebApiFundamentalsERP3.Models.ViewModels;

namespace WebApiFundamentalsERP3.Controllers
{
    [RoutePrefix("api/urun")]
    public class ProductController : ApiController
    {
        [Route("")]
        [HttpGet]
        public ResponseData GetAll()
        {
            try
            {
                var db = new NorthwindEntities();
                var urunler = db.Products.Select(x => new ProductViewModel
                {
                    productId = x.ProductID,
                    categoryId = x.CategoryID,
                    categoryName = x.Category.CategoryName,
                    discontinued = x.Discontinued,
                    productName = x.ProductName,
                    quantityPerUnit = x.QuantityPerUnit,
                    reorderLevel = x.ReorderLevel,
                    supplierId = x.SupplierID,
                    unitPrice = x.UnitPrice,
                    unitsInStock = x.UnitsInStock,
                    unitsOnOrder = x.UnitsOnOrder
                }).ToList();
                return new ResponseData
                {
                    Success = true,
                    Data = urunler
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
        [HttpGet]
        [Route("{catid:int}")]
        public ResponseData GetByCategory(int catid)
        {
            try
            {
                var db = new NorthwindEntities();
                var category = db.Categories.Find(catid);
                if (category == null)
                {
                    return new ResponseData
                    {
                        Success = false,
                        Message = "Kategori bulunamadı"
                    };
                }
                var urunler = category.Products.Select(x => new ProductViewModel
                {
                    productId = x.ProductID,
                    categoryId = x.CategoryID,
                    categoryName = x.Category.CategoryName,
                    discontinued = x.Discontinued,
                    productName = x.ProductName,
                    quantityPerUnit = x.QuantityPerUnit,
                    reorderLevel = x.ReorderLevel,
                    supplierId = x.SupplierID,
                    unitPrice = x.UnitPrice,
                    unitsInStock = x.UnitsInStock,
                    unitsOnOrder = x.UnitsOnOrder
                }).ToList();
                return new ResponseData
                {
                    Success = true,
                    Data = urunler
                };
            }
            catch (Exception ex)
            {
                return new ResponseData
                {
                    Success = false,
                    Message = $"Hata oluştu {ex.Message}"
                };
            }
        }
        [Route("detay/{id:int:min(1)}")]
        [HttpGet]
        public ResponseData Detail(int id)
        {
            try
            {
                var db = new NorthwindEntities();
                var urun = db.Products.Find(id);
                if (urun == null)
                {
                    return new ResponseData
                    {
                        Success = false,
                        Message = "Ürün bulunamadı"
                    };
                }
                return new ResponseData
                {
                    Success = true,
                    Data = new
                    {
                        urun.QuantityPerUnit,
                        urun.Category.CategoryName,
                        urun.Discontinued,
                        urun.ProductName,
                        urun.ReorderLevel,
                        urun.SupplierID,
                        urun.UnitPrice,
                        urun.UnitsInStock,
                        urun.UnitsOnOrder,
                        urun.CategoryID,
                        totalOrdersCount = urun.Order_Details.Count,
                        suppliersCompany = urun.Supplier.CompanyName
                    }
                };
            }
            catch (Exception ex)
            {
                return new ResponseData
                {
                    Success = false,
                    Message = $"Hata oluştu {ex.Message}"
                };
            }
        }

        [HttpPost]
        [Route("ekle")]
        public ResponseData Insert([FromBody]ProductViewModel model)
        {
            if (model == null)
                return new ResponseData
                {
                    Success = false,
                    Message = "model bulunamadı"
                };
            try
            {
                var db = new NorthwindEntities();
                db.Products.Add(new Product
                {
                    CategoryID = model.categoryId,
                    Discontinued = model.discontinued,
                    ProductName = model.productName,
                    UnitPrice = model.unitPrice,
                    UnitsInStock = model.unitsInStock
                });
                db.SaveChanges();
                return new ResponseData
                {
                    Success = true,
                    Message = $"{model.productName} isimli ürün başarıyla eklenmiştir"
                };
            }
            catch (Exception ex)
            {
                return new ResponseData
                {
                    Success = false,
                    Message = $"Ürün ekleme işleminde hata oluştu {ex.Message}"
                };
            }

        }

        [HttpPost]
        [Route("sil/{id}")]
        public ResponseData Delete(int id)
        {
            try
            {
                var db = new NorthwindEntities();
                var silinecek = db.Products.Find(id);
                if (silinecek == null)
                    return new ResponseData
                    {
                        Success = false,
                        Message = "Urun bulunamadı"
                    };
                db.Products.Remove(silinecek);
                db.SaveChanges();
                return new ResponseData
                {
                    Success = true,
                    Message = $"{silinecek.ProductName} adlı ürün başarıyla silinmiştir"
                };
            }
            catch (Exception ex)
            {
                return new ResponseData
                {
                    Success = false,
                    Message = $"Bir hata oluştu {ex.Message}"
                };
            }
        }

        [HttpPost]
        [Route("guncelle")]
        public ResponseData Update([FromBody] ProductViewModel model)
        {
            if (model == null)
                return new ResponseData
                {
                    Success = false,
                    Message = "Model bulunamadı"
                };
            try
            {
                var db = new NorthwindEntities();
                var urun = db.Products.Find(model.productId);
                if (urun == null)
                    return new ResponseData
                    {
                        Success = false,
                        Message = "Ürün bulunamadı"
                    };
                urun.ProductName = model.productName;
                urun.UnitPrice = model.unitPrice;
                urun.CategoryID = model.categoryId;
                urun.Discontinued = model.discontinued;
                db.SaveChanges();
                return new ResponseData
                {
                    Success = true,
                    Message = $"{model.productName} adlı ürün güncellenmiştir"
                };
            }
            catch (Exception ex)
            {
                return new ResponseData
                {
                    Success = false,
                    Message = $"Bir hata oluştu {ex.Message}"
                };
            }

        }
    }
}
