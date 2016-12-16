using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using ToDoWebApp.Properties;

namespace ToDoWebApp.Models
{
    public class DocDBContext
    {
        private static string databaseName = "ToDoDB";
        private static string databaseCollectionName = "ToDoComments";

        private static readonly string EndpointUri = ConfigurationManager.AppSettings["DOCDB_URL"];
        private static readonly string PrimaryKey = ConfigurationManager.AppSettings["DOCDB_KEY"];

        private DocumentClient client;

        //JsonConvert.SerializeObject(itm, Formatting.Indented));

        public DocDBContext()
        {
            // Create a new instance of the DocumentClient
            this.client = new DocumentClient(new Uri(EndpointUri), PrimaryKey);
        }

        public List<ToDoComment> GetComments(Guid gid)
        {
            // Set some common query options
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            // Run a simple query via LINQ. DocumentDB indexes all properties, so queries can be completed efficiently and with low latency.
            // Here we find the Andersen family via its LastName
            IQueryable<ToDoComment> query = this.client.CreateDocumentQuery<ToDoComment>(
                UriFactory.CreateDocumentCollectionUri(databaseName, databaseCollectionName), queryOptions)
                .Where(e => e.ToDoGId == gid);

            return query.ToList();
        }

        public async Task InsertComment(ToDoComment itm)
        {
            await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseName, databaseCollectionName), itm);
        }

        public async Task InitDB()
        {
            await createDatabaseIfNotExists(databaseName);

            await createDocumentCollectionIfNotExists(databaseName, databaseCollectionName);
        }

        private async Task createDocumentCollectionIfNotExists(string databaseName, string collectionName)
        {
            try
            {
                await client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName));
            }
            catch (DocumentClientException de)
            {
                // If the document collection does not exist, create a new collection
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    DocumentCollection collectionInfo = new DocumentCollection();
                    collectionInfo.Id = collectionName;

                    // Optionally, you can configure the indexing policy of a collection. Here we configure collections for maximum query flexibility 
                    // including string range queries. 
                    collectionInfo.IndexingPolicy = new IndexingPolicy(new RangeIndex(DataType.String) { Precision = -1 });

                    // DocumentDB collections can be reserved with throughput specified in request units/second. 1 RU is a normalized request equivalent to the read
                    // of a 1KB document.  Here we create a collection with 400 RU/s. 
                    await this.client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(databaseName),
                        new DocumentCollection { Id = collectionName },
                        new RequestOptions { OfferThroughput = 400 });
                }
                else
                {
                    throw;
                }
            }
        }

        private async Task createDatabaseIfNotExists(string databaseName)
        {
            // Check to verify a database with the id=FamilyDB does not exist
            try
            {
                await client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(databaseName));
            }
            catch (DocumentClientException de)
            {
                // If the database does not exist, create a new database
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    await client.CreateDatabaseAsync(new Database { Id = databaseName });
                }
                else
                {
                    throw;
                }
            }
        }
    }
}