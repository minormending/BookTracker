### **Day 1: Setting Up the Project and Exploring Modern C# Fundamentals**

---

### **Overview**  
The first day is dedicated to setting up the foundational structure of the **Book Tracker API** project and learning **modern C# features** to improve code quality, maintainability, and scalability. This lesson introduces nullable reference types, async/await, pattern matching, and other modern C# techniques, emphasizing when to use (and not use) these features effectively.

---

### **Objectives for Day 1**
1. Set up an ASP.NET Core Web API project.  
2. Learn and apply **modern C# features**:
   - Nullable reference types.
   - Async/await.
   - Pattern matching.
   - Local functions.
3. Write clear, maintainable, and production-ready code by using appropriate tools and techniques.  
4. Understand scenarios where certain techniques are not ideal or overkill.

---

### **Lesson Structure**

#### **Step 1: Setting Up the Project**
**Goal**: Initialize the project and ensure the development environment is ready.  

1. **Create a New Project**  
   Use the `.NET CLI` to create an ASP.NET Core Web API project:  
   ```bash
   dotnet new webapi -n BookTracker
   cd BookTracker
   ```

2. **Run the Project**  
   Verify the project is set up correctly:  
   ```bash
   dotnet run
   ```
   - Open a browser and navigate to `http://localhost:5000/swagger` to see the default Swagger UI.

3. **Enable Nullable Reference Types**  
   Edit the `.csproj` file to enable nullable reference types:  
   ```xml
   <Nullable>enable</Nullable>
   ```
   This ensures stricter null safety checks by the compiler.

4. **Add NuGet Packages**  
   Install dependencies that will be useful later:  
   - **HtmlAgilityPack** for parsing Library Genesis:  
     ```bash
     dotnet add package HtmlAgilityPack
     ```
   - **Swashbuckle.AspNetCore** for Swagger customization:  
     ```bash
     dotnet add package Swashbuckle.AspNetCore
     ```

---

#### **Step 2: Understanding and Using Nullable Reference Types**

**Concept**:  
In traditional C#, reference types like `string` could always be `null`, potentially causing runtime exceptions. Modern C# allows explicit nullability using nullable reference types (`string?`).

**Key Use Cases**:
- Use `string?` for properties or parameters that might legitimately be `null`.  
- Use non-nullable `string` for properties or parameters where `null` is not allowed.  

**Example**:  
In the `Book` class, use `string?` for the `Author` field, as it's possible for some books to not have a known author.  

```csharp
public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty; // Compiler ensures Title is never null.
    public string? Author { get; set; } // Nullable: Some books may lack an author.
    public bool IsCompleted { get; set; }
}
```

**When *Not* to Use Nullable Reference Types**:
- When dealing with value types (e.g., `int`, `bool`), since they cannot be `null` unless explicitly marked nullable (`int?`).
- For properties or fields where `null` has no semantic meaning (e.g., `Title` in this case).

**Task**:  
- Practice creating a nullable and non-nullable property in the `Book` model.

---

#### **Step 3: Implementing Async/Await**

**Concept**:  
Async/await allows methods to run asynchronously, improving scalability and responsiveness by not blocking the calling thread.  

**Key Use Cases**:
- Use async/await for I/O-bound tasks, such as database operations or network requests.
- Avoid async for CPU-bound tasks unless you explicitly need to offload work to another thread.

**Example**:  
Implement an asynchronous method to simulate fetching books:  
```csharp
public async Task<List<Book>> GetBooksAsync()
{
    await Task.Delay(1000); // Simulate network latency or database call.
    return new List<Book>(); // Return an empty list for now.
}
```

**When *Not* to Use Async/Await**:
- For short, synchronous operations that don't involve I/O (e.g., simple data parsing or calculations).  
- Adding async unnecessarily increases complexity.

**Task**:  
- Create a method in `BookService` to asynchronously return a list of books.  

---

#### **Step 4: Using Pattern Matching**

**Concept**:  
Pattern matching simplifies complex conditional logic and makes code more expressive.  

**Key Use Cases**:
- Replace `if-else` chains with `switch` expressions.  
- Handle nested object structures using property patterns.  

**Example**:  
Implement a method to categorize books by genre:  
```csharp
public string GetBookCategory(string genre) =>
    genre switch
    {
        "Fiction" => "Entertainment",
        "Non-Fiction" => "Education",
        "Mystery" => "Thriller",
        _ => "General"
    };
```

**When *Not* to Use Pattern Matching**:
- Avoid overusing pattern matching for simple conditions, as it can make the code harder to read.  

**Task**:  
- Add a method in `BookService` to classify books by genre.

---

#### **Step 5: Writing Local Functions**

**Concept**:  
Local functions are functions defined inside another method. They are useful for encapsulating logic that’s only relevant to that method.  

**Key Use Cases**:
- When the logic is short, reusable within the method, and doesn't clutter the parent method.

**Example**:  
Encapsulate helper logic for calculating statistics:  
```csharp
public (int TotalBooks, int CompletedBooks) CalculateStats(List<Book> books)
{
    int CountCompletedBooks() => books.Count(b => b.IsCompleted);

    return (books.Count, CountCompletedBooks());
}
```

**When *Not* to Use Local Functions**:
- Avoid local functions when they grow too large or need to be reused across multiple methods. In such cases, refactor them into standalone methods.  

**Task**:  
- Add a local function inside a statistics-calculation method in `BookService`.

---

#### **Step 6: Putting It All Together**

**Objective**:  
Combine all the learned concepts into a simple `BookService` and a `BooksController` for basic API functionality.  

1. **BookService**:  
   ```csharp
   public class BookService
   {
       private readonly List<Book> _books = new();

       public async Task<List<Book>> GetBooksAsync()
       {
           await Task.Delay(500); // Simulate database fetch
           return _books;
       }

       public void AddBook(string title, string? author)
       {
           _books.Add(new Book
           {
               Id = _books.Count + 1,
               Title = title,
               Author = author,
               IsCompleted = false
           });
       }
   }
   ```

2. **BooksController**:  
   ```csharp
   [ApiController]
   [Route("api/[controller]")]
   public class BooksController : ControllerBase
   {
       private readonly BookService _bookService;

       public BooksController(BookService bookService)
       {
           _bookService = bookService;
       }

       [HttpGet]
       public async Task<IActionResult> GetBooks()
       {
           var books = await _bookService.GetBooksAsync();
           return Ok(books);
       }

       [HttpPost]
       public IActionResult AddBook([FromBody] Book book)
       {
           _bookService.AddBook(book.Title, book.Author);
           return Ok();
       }
   }
   ```

---

### **Deliverables for Day 1**
1. A functional ASP.NET Core Web API project.  
2. A `Book` model demonstrating nullable reference types.  
3. A `BookService` with async methods, pattern matching, and local functions.  
4. A `BooksController` with basic CRUD functionality.

---

By the end of Day 1, the project will be initialized with a solid foundation, ready for more advanced features like database integration and Library Genesis integration.