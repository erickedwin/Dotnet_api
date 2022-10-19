using Microsoft.AspNetCore.Mvc;
using apinetcore5.Models;
namespace apinetcore5.Controllers;
using System;  
using System.Collections.Generic;
// agregados para EXCEL
using System.Data;
using System.Text;
// EXCEL - CLOSEDXML EXTENSION
using ClosedXML.Excel;
// agregados para PDF
// PDF - SYNCFUSION.PDF.NET.CORE EXTENSION
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using Syncfusion.Pdf.Interactive;




[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{


    private readonly d3rshp0o2v8k4vContext _DBContext;

    public ProductController(d3rshp0o2v8k4vContext dBContext)
    {
       this._DBContext=dBContext;
    }

    //Obtener todos los registros de la tabla MockData
    [HttpGet("GetAll")]
    public IActionResult Get()
    {
      var product=this._DBContext.MockData.ToList();
      return Ok(product);
    }

    //Buscar por ID en la tabla MockData
    [HttpGet("GetbyCode/{code:int}")]
    public IActionResult GetbyCode(int code)
    {
      var product=this._DBContext.MockData.FirstOrDefault(o=>o.Id==code);
      return Ok(product);
    }

     //Buscar por Nombre CON SENSIBILIDAD
    [HttpGet("{Nombre_Producto:alpha}")]
    public async Task<IEnumerable<MockDatum>> Search(string Nombre_Producto)
    {
        IEnumerable<MockDatum> query = _DBContext.MockData.Where(e =>e.ProductName.ToLower() ==  Nombre_Producto.ToLower());
        if ((query != null))
        {
          return query;
        }
        else{
          return Enumerable.Empty<MockDatum>().ToList();
        }
    }

    //FILTRAR POR CARACTER O CADENA DE CARACTERES
     [HttpGet("GetbycharacterString/{caracter:alpha}")]
    public IActionResult GetbycharacterString(string caracter)
    {
       try{
          var productos=this._DBContext.MockData.ToList();
          if(caracter != null){
             productos = productos.Where(o=>o.ProductName.ToLower().IndexOf(caracter)>-1).ToList();
          }
          return Ok(productos);
        }catch
        {
          return BadRequest("Error.");
        }
    }

    //FILTRO EN CANTIDAD

    [HttpGet("getbyvalues{valmax:decimal}/{valmin:decimal}")]
   // [HttpGet("getbyvalues/{valmax:decimal}")]
    public IActionResult getbyvalues(decimal valmax, decimal valmin)
    //public IActionResult getbyvalues(decimal valmax)
    {
      if(valmax == null){
            valmax = 1000.00m;
          } 
      if(valmin == null){
            valmin = 0.00m;
          }
          
       try{
        var productos2=this._DBContext.MockData.ToList();
          productos2 = productos2.Where(o=>o.Lowprices<valmax).ToList();
          productos2 = productos2.Where(o=>o.Lowprices>valmin).ToList();
          //productos2 = productos2.Where(o=>o).ToList();
          return Ok(productos2);
        }catch
        {
          return BadRequest("Error.");
        }
    }
    [HttpGet("getbystringvalue{valmax1:double}/{valmin1:double}")]
    public IActionResult getbystringvalue(double valmax1, double valmin1)
    {
      if(valmax1 == null){
            valmax1 = 1000.00;
          }
          if(valmin1 == null){
            valmin1 = 0.00;
          }
       try{
          var productos2=this._DBContext.MockData.ToList();
             productos2 = productos2.Where(o=>Convert.ToDouble(o.Price.Remove(0,1))>=valmin1).ToList();
             //productos2 = productos2.Where(o=>Convert.ToDouble(o.Price.Remove(0,1))<=valmax1).ToList();
             // var product=this._DBContext.MockData.FirstOrDefault(o=>o.Id==code);

           //  string word = "123.23";
            //Console.WriteLine(Convert.ToDouble(word));
          return Ok(productos2);
        }catch
        {
          return BadRequest("Error.");
        }
    }
    // EXPORTANDO EXCEL CON PAQUETE ClosedXML
    // https://www.nuget.org/packages/ClosedXML
     [HttpGet("ExportToExcel")]
     public IActionResult ExportToExcel()
     {
          var testdata=this._DBContext.MockData.ToList();
         //using System.Data;
         DataTable dt = new DataTable("Grid");
         dt.Columns.AddRange(new DataColumn[4] { new DataColumn("Id"),
                                     new DataColumn("ProductName"), 
                                     new DataColumn("Price"),
                                      new DataColumn("Lowprices")});
              
         foreach (var emp in testdata)
         {
             dt.Rows.Add(emp.Id,emp.ProductName,emp.Price,emp.Lowprices);
         }
         //using ClosedXML.Excel;
         using (XLWorkbook wb = new XLWorkbook())
         {
             wb.Worksheets.Add(dt);
             using (MemoryStream stream = new MemoryStream())
             {
                 wb.SaveAs(stream);
                 return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Mockdatafile.xlsx");
             }
         }
     }

     //EXPORTAR A EXCEL SIN PAQUETES, PURO CODE

     [HttpGet("ExportRecordtoExcelFase2")]
     public ActionResult ExportRecordtoExcelFase2()
     { 
       var productosmock=this._DBContext.MockData.ToList();
         StringBuilder str = new StringBuilder();
         str.Append("<table border=`" + "1px" + "`b>");
         str.Append("<tr>");
         str.Append("<td><b><font face=Arial Narrow size=3>Id</font></b></td>");
         str.Append("<td><b><font face=Arial Narrow size=3>Product</font></b></td>");
         str.Append("<td><b><font face=Arial Narrow size=3>Price_String</font></b></td>");
         str.Append("<td><b><font face=Arial Narrow size=3>Price_Double</font></b></td>");
         str.Append("</tr>");
         //foreach (Mock val in obj)
          foreach (MockDatum val in productosmock)
         {
             str.Append("<tr>");
             str.Append("<td><font face=Arial Narrow size=" + "14px" + ">" + val.Id + "</font></td>");
             str.Append("<td><font face=Arial Narrow size=" + "14px" + ">" + val.ProductName + "</font></td>");
             str.Append("<td><font face=Arial Narrow size=" + "14px" + ">" + val.Price + "</font></td>");
             str.Append("<td><font face=Arial Narrow size=" + "14px" + ">" + val.Lowprices + "</font></td>");
             str.Append("</tr>");
         }
         str.Append("</table>");
         HttpContext.Response.Headers.Add("content-disposition", "attachment; filename=Information" + DateTime.Now.Year.ToString() + ".xls");
         this.Response.ContentType = "application/vnd.ms-excel";
         byte[] temp = System.Text.Encoding.UTF8.GetBytes(str.ToString());
         return File(temp, "application/vnd.ms-excel");
     }  

     //EXPORTAR A PDF CON EL PAQUETE ASYNCFUSION

     [HttpGet("ExportToPDF_FASE1")]
     public IActionResult ExportToPDF_FASE1()
     //static async Task ExportToPDF_FASE1()
     { 
       var productosmock=this._DBContext.MockData.ToList();
       PdfDocument documentopdf = new PdfDocument();
       documentopdf.PageSettings.Margins.All = 0;
      PdfPage page = documentopdf.Pages.Add();
      PdfGraphics g = page.Graphics;
      PdfGrid pdfGrid = new PdfGrid();
      DataTable dataTable = new DataTable();

      //Add columns to the DataTable
/*       dataTable.Columns.Add("ID");
      dataTable.Columns.Add("Product");
      dataTable.Columns.Add("Price_String");
      dataTable.Columns.Add("Price_Double"); */
      //Creating font instances
      PdfFont headerFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 35);
      PdfFont subHeadingFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 16);
      //Dibujando contenido en PDF
       g.DrawRectangle(new PdfSolidBrush(Color.FromArgb(255, 0, 0, 0)), new Syncfusion.Drawing.RectangleF(10, 63, 140, 35));
       g.DrawString("Id", subHeadingFont, new PdfSolidBrush(Color.FromArgb(255, 255, 255, 255)), new Syncfusion.Drawing.PointF(15, 70));
      g.DrawRectangle(new PdfSolidBrush(Color.FromArgb(255, 0, 0, 0)), new Syncfusion.Drawing.RectangleF(10, 63, 140, 35));
       g.DrawString("Product", subHeadingFont, new PdfSolidBrush(Color.FromArgb(255, 255, 255, 255)), new Syncfusion.Drawing.PointF(15, 70));
       g.DrawRectangle(new PdfSolidBrush(Color.FromArgb(255, 0, 0, 0)), new Syncfusion.Drawing.RectangleF(10, 63, 140, 35));
       g.DrawString("Price_String", subHeadingFont, new PdfSolidBrush(Color.FromArgb(255, 255, 255, 255)), new Syncfusion.Drawing.PointF(15, 70));
       g.DrawRectangle(new PdfSolidBrush(Color.FromArgb(255, 0, 0, 0)), new Syncfusion.Drawing.RectangleF(10, 63, 140, 35));
       g.DrawString("Price_Double", subHeadingFont, new PdfSolidBrush(Color.FromArgb(255, 255, 255, 255)), new Syncfusion.Drawing.PointF(15, 70));
        foreach (MockDatum val in productosmock)
         {
             dataTable.Rows.Add(new object[]{val.Id,val.ProductName,val.Price,val.Lowprices});
         }
         pdfGrid.DataSource = dataTable;
         pdfGrid.Draw(page, new PointF(10, 10));
         //documentopdf.Save("datain.pdf");
         MemoryStream ms = new MemoryStream();
         documentopdf.Save(ms);
         ms.Position = 0;
        FileStreamResult fileStreamResult = new FileStreamResult(ms, "application/pdf");
            fileStreamResult.FileDownloadName = "Sample.pdf";
            return fileStreamResult;
     } 

        //BORRAR UN REGISTRO
        [HttpDelete("Remove{code:int}")]
    public IActionResult Remove(int code)
    {
      var product=this._DBContext.MockData.FirstOrDefault(o=>o.Id==code);
      if(product!=null){
        this._DBContext.Remove(product);
        this._DBContext.SaveChanges();
       // _DBContext.(product);
        return Ok(true);
      }
      return Ok(false);
    }
        [HttpPost("Create")]
    public IActionResult Create([FromBody] MockDatum _product)
    {
      var product=this._DBContext.MockData.FirstOrDefault(o=>o.Id==_product.Id);
      if(product!=null){
        product.ProductName=_product.ProductName;
        product.Price=_product.Price;
      }else{
        this._DBContext.MockData.Add(_product);
        this._DBContext.SaveChanges();
      }
      return Ok(true);
    }
}
