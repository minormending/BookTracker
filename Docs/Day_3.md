### **Day 3: Advanced Data Validation**

---

### **Overview**  
Day 3 focuses on ensuring data integrity through advanced validation techniques in the **Book Tracker API**. Developers will use **data annotations** for straightforward validation, implement custom validation attributes for more complex requirements, and learn how to integrate validation into the API pipeline. Additionally, error handling best practices will be explored to ensure clear communication of validation errors to API consumers.

---

### **Objectives**
1. Understand and apply **data annotations** for property-level validation.  
2. Learn how to implement **custom validation attributes** for more specific rules.  
3. Integrate validation into the API pipeline using **ModelState**.  
4. Implement structured error handling and responses for validation failures.  
5. Discuss scenarios when **not** to use built-in validation mechanisms.  

---

### **Lesson Structure**

---

#### **Step 1: Understanding and Applying Data Annotations**  
**Goal**: Use out-of-the-box data annotations to enforce property-level validation rules.

1. **What Are Data Annotations?**  
   Data annotations are attributes applied to model properties to specify validation rules directly in the code. They are part of the `System.ComponentModel.DataAnnotations` namespace.

2. **Applying Basic Annotations**:  
   Modify the `Book` model to include validation rules using annotations.  
   ```csharp
   using System.ComponentModel.DataAnnotations;

   public class Book
   {
       public int Id { get; set; }

       [Required(ErrorMessage = "The Title is required.")]
       [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
       public string Title { get; set; } = string.Empty;

       [StringLength(50, ErrorMessage = "Author name cannot exceed 50 characters.")]
       public string? Author { get; set; }

       [Range(1, 10000, ErrorMessage = "Pages must be between 1 and 10,000.")]
       public int Pages { get; set; }

       public bool IsCompleted { get; set; }
   }
   ```

3. **Validation Example**:  
   - A valid `Book` object:  
     ```json
     {
       "title": "Valid Book Title",
       "author": "Author Name",
       "pages": 300,
       "isCompleted": false
     }
     ```
   - An invalid `Book` object:  
     ```json
     {
       "title": "",
       "author": "This author name is deliberately way too long to pass validation.",
       "pages": 0,
       "isCompleted": false
     }
     ```

4. **When *Not* to Use Data Annotations**:  
   - For rules that require interaction with external services (e.g., checking if a book exists in an external database).  
   - When the validation logic spans multiple properties (e.g., ensuring `Pages > 100` only if `IsCompleted == true`).  

---

#### **Step 2: Custom Validation Attributes**  
**Goal**: Implement domain-specific validation rules by creating custom attributes.

1. **What Are Custom Validation Attributes?**  
   Custom validation attributes allow you to define rules that cannot be expressed using built-in annotations.

2. **Implementing a Custom Attribute**:  
   Create a custom validation attribute to prevent profanity in book titles.  
   ```csharp
   using System.ComponentModel.DataAnnotations;

   public class NoProfanityAttribute : ValidationAttribute
   {
       private static readonly List<string> ProfanityList = new() { "badword1", "badword2" };

       protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
       {
           var title = value as string;
           if (title != null && ProfanityList.Any(word => title.Contains(word, StringComparison.OrdinalIgnoreCase)))
           {
               return new ValidationResult("The title contains prohibited words.");
           }

           return ValidationResult.Success;
       }
   }
   ```

   Update the `Book` model to use this attribute:  
   ```csharp
   [NoProfanity]
   public string Title { get; set; } = string.Empty;
   ```

3. **When *Not* to Use Custom Attributes**:  
   - When the validation involves heavy computation or large datasets. Use service-based validation instead to maintain performance.  
   - For rules that require dynamic data (e.g., fetching prohibited words from a database).  

---

#### **Step 3: Validating Models in the API Pipeline**  
**Goal**: Automatically enforce validation rules when API endpoints are called.

1. **How Model Validation Works in ASP.NET Core**:  
   ASP.NET Core automatically validates models against the data annotations when `[ApiController]` is used. Validation errors are stored in `ModelState`.

2. **Handling Validation in a Controller**:  
   Modify the `BooksController` to validate incoming `Book` objects.  
   ```csharp
   [HttpPost]
   public async Task<IActionResult> AddBook([FromBody] Book book)
   {
       if (!ModelState.IsValid)
       {
           return BadRequest(ModelState); // Returns validation errors to the client
       }

       await _repository.AddBookAsync(book);
       return Ok();
   }
   ```

3. **Structured Error Responses**:  
   Example response for invalid input:  
   ```json
   {
       "title": [
           "The Title is required.",
           "The title contains prohibited words."
       ],
       "pages": [
           "Pages must be between 1 and 10,000."
       ]
   }
   ```

4. **When *Not* to Use ModelState for Validation**:  
   - For highly complex validation logic that depends on asynchronous operations or external services.  
   - When handling validation errors inline with business logic rather than through the API framework.  

---

#### **Step 4: Centralizing Validation Logic**  
**Goal**: Move repetitive validation logic to a central service for better maintainability.

1. **Creating a Validation Service**:  
   Implement a `BookValidationService` to encapsulate validation logic.  
   ```csharp
   public class BookValidationService
   {
       public List<string> ValidateBook(Book book)
       {
           var errors = new List<string>();

           if (string.IsNullOrWhiteSpace(book.Title))
           {
               errors.Add("The Title is required.");
           }

           if (book.Title.Length > 100)
           {
               errors.Add("Title cannot exceed 100 characters.");
           }

           if (book.Pages < 1 || book.Pages > 10000)
           {
               errors.Add("Pages must be between 1 and 10,000.");
           }

           return errors;
       }
   }
   ```

2. **Integrating the Service in the Controller**:  
   ```csharp
   [HttpPost]
   public async Task<IActionResult> AddBook([FromBody] Book book)
   {
       var errors = _validationService.ValidateBook(book);
       if (errors.Any())
       {
           return BadRequest(errors);
       }

       await _repository.AddBookAsync(book);
       return Ok();
   }
   ```

3. **When *Not* to Centralize Validation**:  
   - When the logic is simple and doesn't require reuse across multiple locations.  
   - For very basic projects where introducing additional layers may overcomplicate the code.  

---

### **Summary of Day 3 Deliverables**
1. **Enhanced Book Model**: Includes basic validation with data annotations.  
2. **Custom Validation Attribute**: Prevents profanity in book titles.  
3. **Validation-Enabled API Endpoints**: Automatically handle validation errors in controllers.  
4. **Centralized Validation Service**: A reusable service for validating `Book` objects.

By the end of Day 3, the **Book Tracker API** will have robust validation mechanisms, ensuring data integrity and improving user experience while demonstrating best practices for handling validation in a scalable manner.