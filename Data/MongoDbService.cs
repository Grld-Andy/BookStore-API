using MongoDB.Driver;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookStore.Data;

public class MongoDbService
{
    private readonly IMongoCollection<Book> _booksCollection;

    public MongoDbService(IConfiguration config)
    {
        var client = new MongoClient(config["MongoDB:ConnectionString"]);
        var database = client.GetDatabase(config["MongoDB:DatabaseName"]);
        _booksCollection = database.GetCollection<Book>(config["MongoDB:BooksCollectionName"]);
    }

    public async Task<List<Book>> GetAsync() =>
        await _booksCollection.Find(_ => true).ToListAsync();
    
    public async Task<Book?> GetAsync(string id) =>
        await _booksCollection.Find(x => x.Id.Equals(id)).FirstOrDefaultAsync();
    
    public async Task CreateAsync(Book newBook) =>
        await _booksCollection.InsertOneAsync(newBook);
    
    public async Task<Book?> UpdateAsync(string id, Book updatedBook)
    {
        await _booksCollection.ReplaceOneAsync(x => x.Id.Equals(id), updatedBook);
        return await GetAsync(id);
    }
    
    public async Task RemoveAsync(string id) =>
        await _booksCollection.DeleteOneAsync(x => x.Id.Equals(id));
}