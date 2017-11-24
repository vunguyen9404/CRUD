using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVDShop.Models
{
    public class SqlConnection
    {
        public SQLitePCL.SQLiteConnection conn { get; set; }
        public string path { get; set; }
        public void LoadDatabase()
        {
            path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "db.sqlite");
            conn = new SQLitePCL.SQLiteConnection(path);

            string createUser = @"CREATE TABLE IF NOT EXISTS User (
                            UserID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                            Username VARCHAR(100) NOT NULL,
                            UserPassword VARCHAR(100) NOT NULL,
                            Fullname VARCHAR(100),
                            Email VARCHAR(100),
                            NumberPhone VARCHAR(20),
                            Address VARCHAR(200)
                        );";
            string createCategory = @"CREATE TABLE IF NOT EXISTS Category (
                            CategoryID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                            CategoryName VARCHAR(100) NOT NULL
                        );";

            string createSubCategory = @"CREATE TABLE IF NOT EXISTS SubCategory (
                            SubCategoryID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                            SubCategoryName  vARCHAR(100) NOT NULL,
                            CategoryID INTEGER,
                            FOREIGN KEY(CategoryID) REFERENCES Category(CategoryID)
                        );";

            string createProduct = @"CREATE TABLE IF NOT EXISTS Product(
                            ProductID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                            ProductName VARCHAR(100) NOT NULL,
                            ProductDesc VARCHAR(500),
                            ProductPrice DOUBLE NOT NULL,
                            ProductImage VARCHAR(500),
                            ProductTrailer VARCHAR(500),
                            CategoryID INTEGER,
                            SubCategoryID INTEGER,
                            FOREIGN KEY (CategoryID) REFERENCES Category(CategoryID),
                            FOREIGN KEY (SubCategoryID) REFERENCES SubCategory(SubCategoryID)
                        );";

            using (var statement = conn.Prepare("BEGIN TRANSACTION"))
            {
                statement.Step();
            }
            using (var statement = conn.Prepare(createUser))
            {
                statement.Step();
            }
            using (var statement = conn.Prepare(createCategory))
            {
                statement.Step();
            }
            using (var statement = conn.Prepare(createSubCategory))
            {
                statement.Step();
            }
            using (var statement = conn.Prepare(createProduct))
            {
                statement.Step();
            }
            using (var statement = conn.Prepare("COMMIT TRANSACTION"))
            {
                statement.Step();
            }
            
        }

        public void AddCategory(Category category)
        {
            try
            {
                string sql = @"INSERT INTO Category (CategoryName) VALUES (?);";
                using (var statement = conn.Prepare(sql))
                {
                    statement.Bind(1, category.CategoryName);
                    statement.Step();
                }
            } catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddSubCategory (SubCategory subCategory)
        {
            try
            {
                string sql = @"INSERT INTO SubCategory (SubCategoryName, CategoryID) VALUES (?,?);";
                using (var statement = conn.Prepare(sql))
                {
                    statement.Bind(1, subCategory.SubCategoryName);
                    statement.Bind(2, subCategory.Category.CategoryID);
                    statement.Step();
                }
            } catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddUser(User user)
        {
            try
            {
                string sql = @"INSERT INTO User (Username, UserPassword, Fullname, Email, NumberPhone, Address) VALUES (?,?,?,?,?,?);";
                using (var statement = conn.Prepare(sql))
                {
                    statement.Bind(1, user.Username);
                    statement.Bind(2, user.UserPassword);
                    statement.Bind(3, user.Fullname);
                    statement.Bind(4, user.Email);
                    statement.Bind(5, user.NumberPhone);
                    statement.Bind(6, user.Address);

                    statement.Step();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddProduct(Product product)
        {
            try
            {
                string sql = @"INSERT INTO Product (ProductName, ProductDesc, ProductPrice, ProductImage, ProductTrailer, CategoryID, SubCategoryID) VALUES (?,?,?,?,?,?,?);";
                using (var statement = conn.Prepare(sql))
                {
                    statement.Bind(1, product.ProductName);
                    statement.Bind(2, product.ProductDesc);
                    statement.Bind(3, product.ProductPrice);
                    statement.Bind(4, product.ProductImage);
                    statement.Bind(5, product.ProductTrailer);
                    statement.Bind(6, product.Category.CategoryID);
                    statement.Bind(7, product.SubCategory.SubCategoryID);
                    statement.Step();
                }
            } catch (Exception ex)
            {
                throw ex;
            }
        }

        //Delete

        public void DeleteCategory(Category category)
        {
            try
            {
                string sql = @"DELETE FROM Category WHERE CategoryID = ?;";
                string sql2 = @"DELETE FROM SubCategory WHERE CategoryID = ?;";
                string sql3 = @"DELETE FROM Product WHERE CategoryID = ?;";
                using (var statement = conn.Prepare("BEGIN TRANSACTION"))
                {
                    statement.Step();
                }
                using (var statement = conn.Prepare(sql))
                {
                    statement.Bind(1, category.CategoryID);
                    statement.Step();
                }

                using (var statement = conn.Prepare(sql2))
                {
                    statement.Bind(1, category.CategoryID);
                    statement.Step();
                }

                using (var statement = conn.Prepare(sql3))
                {
                    statement.Bind(1, category.CategoryID);
                    statement.Step();
                }
                using (var statement = conn.Prepare("COMMIT TRANSACTION"))
                {
                    statement.Step();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteSubCategory(SubCategory subCategory)
        {
            try
            {
                using (var statement = conn.Prepare("BEGIN TRANSACTION"))
                {
                    statement.Step();
                }
                string sql = @"DELETE FROM SubCategory WHERE SubCategoryID = ?;";
                string sql2 = @"DELETE FROM Product WHERE SubCategoryID = ?;";
                using (var statement = conn.Prepare(sql))
                {
                    statement.Bind(1, subCategory.SubCategoryID);
                    statement.Step();
                }

                using (var statement = conn.Prepare(sql2))
                {
                    statement.Bind(1, subCategory.SubCategoryID);
                    statement.Step();
                }
                using (var statement = conn.Prepare("COMMIT TRANSACTION"))
                {
                    statement.Step();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteProduct(Product product)
        {
            try
            {
                string sql = @"DELETE FROM Product WHERE ProductID = ?;";
                using (var statement = conn.Prepare(sql))
                {
                    statement.Bind(1, product.ProductID);
                    statement.Step();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteUser(User user)
        {
            try
            {
                string sql = @"DELETE FROM User WHERE UserID = ?;";
                using (var statement = conn.Prepare(sql))
                {
                    statement.Bind(1, user.UserID);
                    statement.Step();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Update

        public void UpdateProduct(Product product)
        {
            try
            {
                string sql = @"UPDATE Product SET ProductName = ?, ProductDesc = ?, ProductPrice = ?, ProductImage = ?, ProductTrailer = ?, CategoryID = ?, SubCategoryID = ? WHERE ProductID = ?;";
                using (var statement = conn.Prepare(sql))
                {
                    statement.Bind(1, product.ProductName);
                    statement.Bind(2, product.ProductDesc);
                    statement.Bind(3, product.ProductPrice);
                    statement.Bind(4, product.ProductImage);
                    statement.Bind(5, product.ProductTrailer);
                    statement.Bind(6, product.Category.CategoryID);
                    statement.Bind(7, product.SubCategory.SubCategoryID);
                    statement.Bind(8, product.ProductID);
                    statement.Step();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateSubCategory(SubCategory subCategory)
        {
            try
            {
                string sql = @"UPDATE SubCategory SET SubCategoryName =?, CategoryID = ? WHERE SubCategoryID = ?;";
                using (var statement = conn.Prepare(sql))
                {
                    statement.Bind(1, subCategory.SubCategoryName);
                    statement.Bind(2, subCategory.Category.CategoryID);
                    statement.Bind(3, subCategory.SubCategoryID);
                    statement.Step();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateCategory(Category category)
        {
            try
            {
                string sql = @"UPDATE Category SET CategoryName = ? WHERE CategoryID = ?;";
                using (var statement = conn.Prepare(sql))
                {
                    statement.Bind(1, category.CategoryName);
                    statement.Bind(2, category.CategoryID);
                    statement.Step();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Category> GetListCategory()
        {
            try
            {
                List<Category> list = new List<Category>();
                string sql = @"SELECT * FROM Category;";
                using (var statement = conn.Prepare(sql))
                {
                    while (SQLitePCL.SQLiteResult.ROW == statement.Step())
                    {
                        if (statement[0] == null)
                        {
                            break;
                        }
                        list.Add(new Category()
                        {
                            CategoryID = (int)statement.GetInteger(0),
                            CategoryName = statement.GetText(1)
                        });
                    }
                }
                return list;
            } catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SubCategory> GetListSubCategory()
        {
            try
            {
                List<SubCategory> list = new List<SubCategory>();
                string sql = @"SELECT * FROM SubCategory LEFT JOIN Category ON SubCategory.CategoryID = Category.CategoryID;";
                using (var statement = conn.Prepare(sql))
                {
                    while (SQLitePCL.SQLiteResult.ROW == statement.Step())
                    {
                        if (statement[0] == null)
                        {
                            break;
                        }
                        list.Add(new SubCategory()
                        {
                            SubCategoryID = (int)statement.GetInteger(0),
                            SubCategoryName = statement.GetText(1),
                            Category = new Category()
                            {
                                CategoryID = (int)statement.GetInteger(2),
                                CategoryName = statement.GetText(4)
                            }
                        });
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Product> GetListProduct()
        {
            try
            {
                List<Product> list = new List<Product>();
                string sql = @"SELECT * FROM Product LEFT JOIN Category ON Product.CategoryID = Category.CategoryID LEFT JOIN SubCategory ON Product.SubCategoryID = SubCategory.SubCategoryID;";
                using (var statement = conn.Prepare(sql))
                {
                    while (SQLitePCL.SQLiteResult.ROW == statement.Step())
                    {
                        if (statement[0] == null)
                        {
                            break;
                        }
                        list.Add(new Product()
                        {
                            ProductID = (int) statement.GetInteger(0),
                            ProductName = statement.GetText(1),
                            ProductDesc = statement.GetText(2),
                            ProductPrice = statement.GetFloat(3),
                            ProductImage = statement.GetText(4),
                            ProductTrailer = statement.GetText(5),
                            Category = new Category()
                            {
                                CategoryID = (int) statement.GetInteger(6),
                                CategoryName = statement.GetText(9)
                            },
                            SubCategory = new SubCategory()
                            {
                                SubCategoryID = (int) statement.GetInteger(7),
                                SubCategoryName = statement.GetText(11),
                                Category = new Category()
                                {
                                    CategoryID = (int)statement.GetInteger(6),
                                    CategoryName = statement.GetText(9)
                                }
                            }
                        });
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Category LastCategory()
        {
            try
            {
                Category cate = new Category();
                string sql = @"SELECT * FROM Category ORDER BY ID DESC LIMIT 1";
                using (var statement = conn.Prepare(sql))
                {
                    if (SQLitePCL.SQLiteResult.ROW == statement.Step())
                    {
                        cate = new Category()
                        {
                            CategoryID = (int)statement.GetInteger(0),
                            CategoryName = statement.GetText(1)
                        };
                    }
                }
                return cate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}