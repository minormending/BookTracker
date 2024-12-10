### **Overview of Week 1: Building the Foundation for the Book Tracker API**

---

Week 1 focuses on setting up the **Book Tracker API** project and mastering essential **C# backend development skills** while adopting modern frameworks, tools, and practices. This week provides a strong foundation in creating RESTful APIs, managing data, and integrating popular libraries like **HtmlAgilityPack** and **Entity Framework Core**.

---

### **Objectives for Week 1**
1. Learn modern C# features such as nullable reference types, async/await, pattern matching, and data annotations.  
2. Set up an ASP.NET Core Web API project with clean architecture principles.  
3. Integrate **Entity Framework Core (EF Core)** for database management.  
4. Build CRUD (Create, Read, Update, Delete) operations for the core `Book` entity.  
5. Understand and apply validation techniques using data annotations and custom logic.  
6. Create a solid foundation for adding external data sources, such as **Library Genesis**, in subsequent weeks.  

---

### **Detailed Weekly Plan**

#### **Day 1: Setting Up the Project and Exploring Modern C#**
- **Focus**: Set up the project structure, configure tools, and learn modern C# features.  
- **Activities**: 
  - Initialize an ASP.NET Core Web API project.  
  - Learn nullable reference types, async/await, and pattern matching.  
  - Create a basic `Book` model and a `BookService` class.  
  - Implement a simple API to retrieve and add books in memory.  

#### **Day 2: Database Integration with EF Core**
- **Focus**: Introduce database integration and CRUD operations.  
- **Activities**: 
  - Install and configure EF Core with SQLite as the database.  
  - Create the `BookDbContext` for database interactions.  
  - Apply migrations to set up the database schema.  
  - Implement CRUD operations in a `BookRepository`.  
  - Test API endpoints for CRUD functionality.  

#### **Day 3: Advanced Data Validation**
- **Focus**: Enhance data integrity and enforce validation rules.  
- **Activities**: 
  - Use data annotations for basic validation (e.g., `Required`, `StringLength`).  
  - Implement custom validation logic for domain-specific rules (e.g., no profanity in book titles).  
  - Integrate validation with the API endpoints.  
  - Learn to handle validation errors gracefully in the API responses.  

#### **Day 4: Setting Up Swagger for API Documentation**
- **Focus**: Add and customize API documentation with **Swashbuckle**.  
- **Activities**: 
  - Install Swashbuckle for auto-generating Swagger documentation.  
  - Configure Swagger UI to display API endpoints and models.  
  - Add XML comments to controllers and methods for better API documentation.  
  - Learn best practices for documenting APIs for third-party use.  

#### **Day 5: Introduction to Web Scraping with HtmlAgilityPack**
- **Focus**: Introduce external data integration by learning web scraping techniques.  
- **Activities**: 
  - Install and configure **HtmlAgilityPack**.  
  - Write a simple scraper to extract book data from Library Genesis.  
  - Parse and format the extracted data for integration into the API.  
  - Discuss ethical considerations and best practices for web scraping.  

#### **Day 6: Combining API and Database with External Data**
- **Focus**: Integrate web-scraped data with the database.  
- **Activities**: 
  - Create a method to fetch books from Library Genesis and add them to the local database.  
  - Test the integration by populating the database with scraped data.  
  - Design API endpoints to query and filter books added from external sources.  
  - Discuss the limitations and edge cases of combining external data with local storage.  

#### **Day 7: Review and Polish**
- **Focus**: Review the week’s progress, refactor code, and enhance functionality.  
- **Activities**: 
  - Conduct a code review to identify areas for improvement.  
  - Optimize performance of database queries and web scraping.  
  - Refactor controllers and services for better maintainability.  
  - Deploy the API locally and test all functionalities end-to-end.  

---

### **Deliverables for Week 1**
1. A functional ASP.NET Core Web API project with modern C# features.  
2. A SQLite-backed database integrated with **EF Core**.  
3. Full CRUD support for a `Book` entity with validation.  
4. Basic API documentation using **Swagger**.  
5. A working web scraper for fetching book data from Library Genesis.  
6. A combined API and database system capable of managing and querying book data.  

By the end of Week 1, the **Book Tracker API** will have a robust foundation, with essential backend functionality and integration points ready for more advanced features in the coming weeks.