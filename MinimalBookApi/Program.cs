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
var books = new List<Book> {
    new Book { Id = 1, Title = "�������� 1", Author="asdfasdf"},
    new Book { Id = 2, Title = "���������" },
    new Book { Id = 3, Title = "��������� �����" },
    new Book { Id = 4, Title = "������� ����������������" },
    new Book { Id = 5, Title = "�" },
};

////////////////// ******************** CRUD OPERATIONS ******************** ///////////////////////////

/// ******************** C CREATE ����� MapPOST - ������ ����� ����� 
app.MapPost("/book", (Book book) =>
{
    books.Add(book);
    return books;
});

/// ******************** R READ ����� MapGet - ��������� ���� ������� �� id 
//app.MapGet("/book", async (DataContext context) => {
//    return await context.Books.ToListAsync();
//});

//refactor
app.MapGet("/book", async (DataContext context) => 
    await context.Books.ToListAsync());

/// ******************** R READ ����� MapGet - ��������� ����� ������ �� id
app.MapGet("/book{id}", (int id) => {

    var book = books.Find(b => b.Id == id);
    if (book is null)
        return Results.NotFound("Sorry, this book doesnt't exist");
    return Results.Ok(book);
});

/// ******************** U UPDATE ����� MapGet - 
app.MapPut("/book{id}", (Book updateBook, int id) => {

    var book = books.Find(b => b.Id == id);
    if (book is null)
        return Results.NotFound("Sorry, this book doesnt't exist");
    
    book.Title = updateBook.Title;
    book.Author = updateBook.Author;

    return Results.Ok(books);
});

/// ******************** D DELETE ����� MapGet - 
app.MapDelete("/book{id}", (int id) => {

    var book = books.Find(b => b.Id == id);
    if (book is null)
        return Results.NotFound("Sorry, this book doesnt't exist");
    books.Remove(book);

    return Results.Ok(books);
});

app.Run();

public class Book 
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
}
