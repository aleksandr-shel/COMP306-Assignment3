using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Group31_COMP306_Assignment3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Group31_COMP306_Assignment3.Controllers
{
    public class DynamoDBOperations
    {
        AmazonDynamoDBClient clientDynamoDB;
        protected DynamoDBContext context;
        private static string accessKey = "AKIATVAQEU4Y2MEXAAH2";
        private static string secretKey = "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";
        public DynamoDBOperations()
        {
            clientDynamoDB = new AmazonDynamoDBClient(accessKey, secretKey, RegionEndpoint.CACentral1);
            context = new DynamoDBContext(clientDynamoDB);
        }

        public async Task CreateCommentsTable()
        {
            if (await IsThereTable("Comments"))
            {
                return;
            }
            CreateTableRequest request = new CreateTableRequest
            {
                TableName = "Comments",
                AttributeDefinitions = new List<AttributeDefinition>
                {
                    new AttributeDefinition
                    {
                        AttributeName="MovieTitle",
                        AttributeType="S"
                    },
                    new AttributeDefinition
                    {
                        AttributeName="Time",
                        AttributeType="S"
                    }
                },
                KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement{
                        AttributeName="MovieTitle",
                        KeyType="HASH"
                    },
                    new KeySchemaElement{
                        AttributeName="Time",
                        KeyType="RANGE"
                    }
                },
                BillingMode = BillingMode.PROVISIONED,
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 1,
                    WriteCapacityUnits = 1
                }
            };
            try
            {
                var response = await clientDynamoDB.CreateTableAsync(request);
                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK) Console.WriteLine("Table created");
            } catch (InternalServerErrorException iee)
            {
                Console.WriteLine(iee.Message);
            } catch (LimitExceededException lee)
            {
                Console.WriteLine(lee.Message);
            }
        }

        public async Task CreateRatingsTable()
        {
            if (await IsThereTable("Ratings"))
            {
                return;
            }
            CreateTableRequest request = new CreateTableRequest
            {
                TableName = "Ratings",
                AttributeDefinitions = new List<AttributeDefinition>
                {
                    new AttributeDefinition
                    {
                        AttributeName="MovieTitle",
                        AttributeType="S"
                    },
                    new AttributeDefinition
                    {
                        AttributeName="UserId",
                        AttributeType="N"
                    }
                },
                KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement{
                        AttributeName="MovieTitle",
                        KeyType="HASH"
                    },
                    new KeySchemaElement{
                        AttributeName="UserId",
                        KeyType="RANGE"
                    }
                },
                BillingMode = BillingMode.PROVISIONED,
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 1,
                    WriteCapacityUnits = 1
                }
            };
            try
            {
                var response = await clientDynamoDB.CreateTableAsync(request);
                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK) Console.WriteLine("Table created");
            }
            catch (InternalServerErrorException iee)
            {
                Console.WriteLine(iee.Message);
            }
            catch (LimitExceededException lee)
            {
                Console.WriteLine(lee.Message);
            }
        }

        public async Task CreateMoviesTable()
        {
            if (await IsThereTable("Movies"))
            {
                return;
            }
            CreateTableRequest request = new CreateTableRequest
            {
                TableName = "Movies",
                AttributeDefinitions = new List<AttributeDefinition>
                {
                    new AttributeDefinition
                    {
                        AttributeName="MovieTitle",
                        AttributeType="S"
                    },
                    new AttributeDefinition
                    {
                        AttributeName="UserId",
                        AttributeType="N"
                    }
                },
                KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement{
                        AttributeName="MovieTitle",
                        KeyType="HASH"
                    },
                    new KeySchemaElement{
                        AttributeName="UserId",
                        KeyType="RANGE"
                    }
                },
                BillingMode = BillingMode.PROVISIONED,
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 1,
                    WriteCapacityUnits = 1
                }
            };
            try
            {
                var response = await clientDynamoDB.CreateTableAsync(request);
                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK) Console.WriteLine("Table created");
            }
            catch (InternalServerErrorException iee)
            {
                Console.WriteLine(iee.Message);
            }
            catch (LimitExceededException lee)
            {
                Console.WriteLine(lee.Message);
            }
        }
        public async Task CreateMovieDescription(string movieTitle, int userId, string description, string director)
        {
            await CreateMoviesTable();
            Movie movie = new Movie(movieTitle, userId, description, director);
            await context.SaveAsync<Movie>(movie);
        }

        public async Task CreateComment(string movieTitle, string username, string content, string time = null)
        {
            await CreateCommentsTable();
            Comment comment = new Comment(movieTitle, username, content, time);
            await context.SaveAsync<Comment>(comment);
        }

        public async Task DeleteComment(string movieTitle, string username, string content, string time)
        {
            await CreateCommentsTable();
            Comment comment = new Comment(movieTitle, username, content, time);
            await context.DeleteAsync<Comment>(comment);
        }

        public async Task<List<Comment>> GetMovieComments(string movieTitle){
            var search = context.FromQueryAsync<Comment>(new Amazon.DynamoDBv2.DocumentModel.QueryOperationConfig()
            {
                Filter = new Amazon.DynamoDBv2.DocumentModel.QueryFilter("MovieTitle", Amazon.DynamoDBv2.DocumentModel.QueryOperator.Equal, movieTitle)
            });

            var searchResponse = await search.GetRemainingAsync();
            List<Comment> comments = searchResponse.ToList();
            return comments;
        }

        public async Task<List<Movie>> GetMovie(string movieTitle)
        {
            var search = context.FromQueryAsync<Movie>(new Amazon.DynamoDBv2.DocumentModel.QueryOperationConfig()
            {
                Filter = new Amazon.DynamoDBv2.DocumentModel.QueryFilter("MovieTitle", Amazon.DynamoDBv2.DocumentModel.QueryOperator.Equal, movieTitle)
            });

            var searchResponse = await search.GetRemainingAsync();
            List<Movie> movieObject = searchResponse.ToList();
            return movieObject;
        }

        public async Task<bool> IsThereTable(string tablename)
        {
            ListTablesResponse response = await clientDynamoDB.ListTablesAsync();
            if (response.TableNames.Contains(tablename))
            {
                return true;
            } else
            {
                return false;
            }
        }
    }
}
