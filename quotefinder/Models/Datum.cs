
namespace Models.Datum{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

public record Datum(
 string _id,
 string quoteText,
 string quoteAuthor,
 string quoteGenre,
 int __v
);

public record Pagination(
 int currentPage,
 int nextPage,
 int totalPages
    );

public record Root(
 int statusCode,
 string message,
 Pagination pagination,
 int totalQuotes,
 IReadOnlyList<Datum> data
 );

}