using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/user", (HttpResponse response) => {
  response.Headers.Add("teste", "Hello World!");
  return new {Name = "Stephany Batista", Age = 35};
  
});

app.MapPost("/products", (Product product) => {
  ProductRepository.Add(product);
  return Results.Created($"/products/{product.Code}", product.Code);
});

app.MapPut("/products", (Product product) => {
 var getProduct = ProductRepository.GetByProductCode(product.Code);
 getProduct.Name = product.Name;
 return Results.Ok();  
});

app.MapDelete("/products/{code}", ([FromRoute]string code) => {
  var productRemove = ProductRepository.GetByProductCode(code);
  ProductRepository.DeleteProduct(productRemove);
  return Results.Ok(); 
});

app.MapGet("/products/{code}", ([FromRoute]string code ) => {
  var product =  ProductRepository.GetByProductCode(code);

  if(product != null)
    return Results.Ok(product);

  return Results.NotFound();
});


app.Run();

public static class ProductRepository {
  public static List<Product> Products { get; set; }

  public static void Add(Product product) {
    if (Products== null){
      Products = new List<Product>();
    }
    Products.Add(product);
  }

  public static Product GetByProductCode(string code){
    return Products.Find(p => p.Code == code);
  }

  public static void DeleteProduct(Product product){
     Products.Remove(product);
  }

}

public class Product {
  public string Code {get; set; }
  public string Name {get; set; }

}