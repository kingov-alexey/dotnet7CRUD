using MinimalBookApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


//Initial_state
//var books = new List<Book> {
//    new Book { Id = 1, Title = "название 1", Author="asdfasdf"},
//    new Book { Id = 2, Title = "Гарипотер" },
//    new Book { Id = 3, Title = "Властелин колец" },
//    new Book { Id = 4, Title = "Патерны программирования" },
//    new Book { Id = 5, Title = "Е" },
//};

////////////////// ******************** CRUD OPERATIONS ******************** ///////////////////////////

/// ******************** C CREATE МЕТОД MapPOST - запись одной книги 
app.MapPost("/book", async (DataContext context, Book book) =>
{
    context.Books.Add(book);
    await context.SaveChangesAsync();
    return Results.Ok(await context.Books.ToListAsync());
});

/// ******************** R READ МЕТОД MapGet - Получение всех записей по id 
//app.MapGet("/book", async (DataContext context) => {
//    return await context.Books.ToListAsync();
//});

//refactor
app.MapGet("/book", async (DataContext context) => 
    await context.Books.ToListAsync());

/// ******************** R READ МЕТОД MapGet - Получение одной записи по id
//app.MapGet("/book{id}", (int id) =>
//{

//    var book = books.Find(b => b.Id == id);
//    if (book is null)
//        return Results.NotFound("Sorry, this book doesnt't exist");
//    return Results.Ok(book);
//});

//app.MapGet("/book{id}", (int id) =>
//{

//refactor
app.MapGet("/book{id}", async (DataContext context, int id) =>
    await context.Books.FindAsync(id) is Book book ?
    Results.Ok(book) : 
    Results.NotFound("Sorry, book not found. :("));

/// ******************** U UPDATE МЕТОД MapGet - 
app.MapPut("/book{id}", async (DataContext context, Book updateBook, int id) =>
{

    var book = await context.Books.FindAsync(id);
    if (book is null)
        return Results.NotFound("Sorry, this book doesnt't exist");

    book.Title = updateBook.Title;
    book.Author = updateBook.Author;
    await context.SaveChangesAsync();

    return Results.Ok(await context.Books.ToListAsync());
});

/// ******************** D DELETE МЕТОД MapGet - 
app.MapDelete("/book{id}", async (DataContext context, int id) =>
{

    var book = await context.Books.FindAsync(id);
    if (book is null)
        return Results.NotFound("Sorry, this book doesnt't exist");
    context.Books.Remove(book);
    await context.SaveChangesAsync();

    return Results.Ok(await context.Books.ToListAsync());
});

app.Run();

public class Book 
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
}
