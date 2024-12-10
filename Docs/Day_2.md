### **Day 2: Building Core Functionality and Introducing Database Integration**

---

### **Overview**
Day 2 focuses on introducing **Entity Framework Core (EF Core)** for database integration, adding a relational database to the **Book Tracker API** project, and creating the necessary data models and migration scripts. This day also covers designing clean and scalable APIs, integrating validation, and understanding best practices for working with databases.

---

### **Objectives**
1. Understand and implement EF Core for database management.  
2. Set up a database and connect it to the project.  
3. Learn and apply data validation techniques using **Data Annotations** and custom validation logic.  
4. Explore database migration and seeding with EF Core.  
5. Design and implement CRUD (Create, Read, Update, Delete) operations for the `Book` entity using EF Core.  
6. Identify when **not** to use certain database techniques.

---

### **Lesson Structure**

---

#### **Step 1: Setting Up EF Core**
**Goal**: Integrate EF Core and connect to a SQLite database for simplicity.

1. **Install EF Core Packages**:  
   Use the `.NET CLI` to add the EF Core dependencies:  
   ```bash
   dotnet add package Microsoft.EntityFrameworkCore
   dotnet add package Microsoft.EntityFrameworkCore.Sqlite
   dotnet add package Microsoft.EntityFrameworkCore.Tools
   ```

2. **Configure EF Core in the Project**:
   Add a `BookDbContext` class to manage database operations.  
   ```csharp
   using Microsoft.EntityFrameworkCore;

   public class BookDbContext : DbContext
   {
       public BookDbContext(DbContextOptions<BookDbContext> options) : base(options) { }

       public DbSet<Book> Books { get; set; }
   }
   ```

3. **Register the Context in `Program.cs`**:
   Modify the `Program.cs` file to configure the SQLite connection.  
   ```csharp
   var builder = WebApplication.CreateBuilder(args);

   // Add services to the container.
   builder.Services.AddDbContext<BookDbContext>(options =>
       options.UseSqlite("Data Source=books.db"));

   builder.Services.AddControllers();
   builder.Services.AddEndpointsApiExplorer();
   builder.Services.AddSwaggerGen();

   var app = builder.Build();
   app.UseSwagger();
   app.UseSwaggerUI();

   app.MapControllers();
   app.Run();
   ```

4. **Run the Project**:
   Test to ensure the setup doesn’t produce errors. At this point, the database connection is ready, but the database hasn’t been created yet.

---

#### **Step 2: Database Migrations**
**Goal**: Create a database schema using EF Core’s migration capabilities.

1. **Create a Migration**:
   Use the `.NET CLI` to generate an initial migration.  
   ```bash
   dotnet ef migrations add InitialCreate
   ```

2. **Apply the Migration**:
   Create the database and apply the migration.  
   ```bash
   dotnet ef database update
   ```

3. **Verify the Database**:
   Use a SQLite browser or CLI tool to confirm the `Books` table exists.

4. **When *Not* to Use Migrations**:
   - In production environments where schema updates are handled manually to avoid accidental data loss.
   - For large-scale databases where migrations might not support all required changes (e.g., complex stored procedures).

---

#### **Step 3: Adding Validation to Models**
**Goal**: Ensure data integrity by applying validation rules to the `Book` model.

1. **Use Data Annotations**:
   Modify the `Book` class to include validation attributes.  
   ```csharp
   using System.ComponentModel.DataAnnotations;

   public class Book
   {
       public int Id { get; set; }

       [Required]
       [StringLength(100, ErrorMessage = "Title length can't exceed 100 characters.")]
       public string Title { get; set; } = string.Empty;

       [StringLength(50, ErrorMessage = "Author length can't exceed 50 characters.")]
       public string? Author { get; set; }

       public bool IsCompleted { get; set; }
   }
   ```

2. **Add Custom Validation Logic**:
   Implement a custom validation attribute for the `Title` property.  
   ```csharp
   public class NoProfanityAttribute : ValidationAttribute
   {
       protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
       {
           var title = value as string;
           if (title != null && title.Contains("badword"))
           {
               return new ValidationResult("Profanity is not allowed in the title.");
           }

           return ValidationResult.Success;
       }
   }
   ```

   Apply it to the `Title` property:  
   ```csharp
   [NoProfanity]
   public string Title { get; set; } = string.Empty;
   ```

3. **When *Not* to Use Data Annotations**:
   - For complex validation rules spanning multiple fields or requiring external dependencies.
   - For performance-critical scenarios, as annotations may introduce overhead.

---

#### **Step 4: CRUD Operations with EF Core**
**Goal**: Implement repository methods for creating, reading, updating, and deleting books.

1. **Add Repository Methods**:
   Create a `BookRepository` class to handle database interactions.  
   ```csharp
   public class BookRepository
   {
       private readonly BookDbContext _context;

       public BookRepository(BookDbContext context)
       {
           _context = context;
       }

       public async Task<List<Book>> GetBooksAsync()
       {
           return await _context.Books.ToListAsync();
       }

       public async Task AddBookAsync(Book book)
       {
           _context.Books.Add(book);
           await _context.SaveChangesAsync();
       }

       public async Task<Book?> GetBookByIdAsync(int id)
       {
           return await _context.Books.FindAsync(id);
       }

       public async Task UpdateBookAsync(Book book)
       {
           _context.Books.Update(book);
           await _context.SaveChangesAsync();
       }

       public async Task DeleteBookAsync(int id)
       {
           var book = await GetBookByIdAsync(id);
           if (book != null)
           {
               _context.Books.Remove(book);
               await _context.SaveChangesAsync();
           }
       }
   }
   ```

2. **When *Not* to Use EF Core**:
   - For extremely high-performance requirements, where raw SQL queries may be more efficient.
   - For very simple use cases where a full ORM is unnecessary.

---

#### **Step 5: Testing CRUD Operations**
**Goal**: Test the CRUD methods using the API.

1. **Modify the `BooksController`**:
   Update the controller to use the repository.  
   ```csharp
   [ApiController]
   [Route("api/[controller]")]
   public class BooksController : ControllerBase
   {
       private readonly BookRepository _repository;

       public BooksController(BookRepository repository)
       {
           _repository = repository;
       }

       [HttpGet]
       public async Task<IActionResult> GetBooks()
       {
           var books = await _repository.GetBooksAsync();
           return Ok(books);
       }

       [HttpPost]
       public async Task<IActionResult> AddBook([FromBody] Book book)
       {
           if (!ModelState.IsValid) return BadRequest(ModelState);

           await _repository.AddBookAsync(book);
           return Ok();
       }

       [HttpDelete("{id}")]
       public async Task<IActionResult> DeleteBook(int id)
       {
           await _repository.DeleteBookAsync(id);
           return Ok();
       }
   }
   ```

2. **Test the API**:
   Use Swagger or Postman to test the `GET`, `POST`, and `DELETE` endpoints.

---

### **Deliverables for Day 2**
1. A fully functional SQLite database connected to the API using EF Core.  
2. A `Book` model with validation using data annotations and custom attributes.  
3. A `BookRepository` with CRUD operations implemented.  
4. A working `BooksController` integrated with the repository.

By the end of Day 2, the **Book Tracker API** will have a database-backed architecture and basic functionality, setting the stage for integrating external data sources and advanced features.