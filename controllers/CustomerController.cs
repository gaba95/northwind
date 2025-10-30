using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Linq;
using Northwind.Data;
using Northwind.Models;

public class CustomersController : Controller
{
    private readonly ApplicationDbContext db = new ApplicationDbContext();

    // GET: /Customers/Index
    public ActionResult Index()
    {
        return View(new List<Customer>());
    }

    // POST: Upload Excel
    [HttpPost]
    public ActionResult Upload(HttpPostedFileBase excelFile)
    {
        if (excelFile == null || excelFile.ContentLength == 0)
        {
            ViewBag.Error = "No se seleccionó ningún archivo.";
            return View("Index", new List<Customer>());
        }

        var customers = new List<Customer>();

        try
        {
            IWorkbook workbook = new XSSFWorkbook(excelFile.InputStream);
            ISheet sheet = workbook.GetSheetAt(0);

            var headerRow = sheet.GetRow(0);
            string[] requiredColumns = { "CustomerID", "CompanyName", "ContactName", "ContactTitle", "Address", "City", "Region", "PostalCode", "Country", "Phone", "Fax" };

            for (int i = 0; i < requiredColumns.Length; i++)
            {
                if (headerRow.GetCell(i) == null || headerRow.GetCell(i).StringCellValue != requiredColumns[i])
                {
                    ViewBag.Error = $"La columna {requiredColumns[i]} es requerida.";
                    return View("Index", new List<Customer>());
                }
            }

            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);
                if (row == null) continue;

                customers.Add(new Customer
                {
                    CustomerID = row.GetCell(0)?.ToString(),
                    CompanyName = row.GetCell(1)?.ToString(),
                    ContactName = row.GetCell(2)?.ToString(),
                    ContactTitle = row.GetCell(3)?.ToString(),
                    Address = row.GetCell(4)?.ToString(),
                    City = row.GetCell(5)?.ToString(),
                    Region = row.GetCell(6)?.ToString(),
                    PostalCode = row.GetCell(7)?.ToString(),
                    Country = row.GetCell(8)?.ToString(),
                    Phone = row.GetCell(9)?.ToString(),
                    Fax = row.GetCell(10)?.ToString()
                });

                Session["Customers"] = customers;
            }

            return View("Index", customers);
        }
        catch
        {
            ViewBag.Error = "Error al procesar el archivo Excel.";
            return View("Index", new List<Customer>());
        }
    }

    // POST: Guardar en DB
    [HttpPost]
    public ActionResult SaveToDb()
    {
        var customers = Session["Customers"] as List<Customer>;
        if (customers != null && customers.Any())
        {
            db.Customers.AddRange(customers);
            db.SaveChanges();
            ViewBag.Success = "Datos guardados correctamente.";
        }
        else
        {
            ViewBag.Error = "No hay datos para guardar.";
        }

        // Limpiar sesión
        Session["Customers"] = null;

        return View("Index", new List<Customer>());
    }
}