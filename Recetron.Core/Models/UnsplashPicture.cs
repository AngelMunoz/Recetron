using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Recetron.Core.Models
{

  public class DailyPictureRecord
  {
    [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
    public string? Id { get; set; }
    public UnsplashPicture? Picture { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
  }

  public class UnsplashPicture
  {
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTimeOffset? UpdatedAt { get; set; }

    [JsonPropertyName("width")]
    public long? Width { get; set; }

    [JsonPropertyName("height")]
    public long? Height { get; set; }

    [JsonPropertyName("color")]
    public string? Color { get; set; }

    [JsonPropertyName("downloads")]
    public long? Downloads { get; set; }

    [JsonPropertyName("likes")]
    public long? Likes { get; set; }

    [JsonPropertyName("liked_by_user")]
    public bool? LikedByUser { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("exif")]
    public Exif? Exif { get; set; }

    [JsonPropertyName("location")]
    public Location? Location { get; set; }

    [JsonPropertyName("current_user_collections")]
    public IEnumerable<CurrentUserCollection>? CurrentUserCollections { get; set; }

    [JsonPropertyName("urls")]
    public Urls? Urls { get; set; }

    [JsonPropertyName("links")]
    public Links? Links { get; set; }

    [JsonPropertyName("user")]
    public UnsplashUser? User { get; set; }
  }

  public class CurrentUserCollection
  {
    [JsonPropertyName("id")]
    public long? Id { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("published_at")]
    public DateTimeOffset? PublishedAt { get; set; }

    [JsonPropertyName("last_collected_at")]
    public DateTimeOffset? LastCollectedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTimeOffset? UpdatedAt { get; set; }

    [JsonPropertyName("cover_photo")]
    public dynamic? CoverPhoto { get; set; }

    [JsonPropertyName("user")]
    public dynamic? User { get; set; }
  }

  public class Exif
  {
    [JsonPropertyName("make")]
    public string? Make { get; set; }

    [JsonPropertyName("model")]
    public string? Model { get; set; }

    [JsonPropertyName("exposure_time")]
    public string? ExposureTime { get; set; }

    [JsonPropertyName("aperture")]
    public string? Aperture { get; set; }

    [JsonPropertyName("focal_length")]
    [JsonIgnore]
    public long? FocalLength { get; set; }

    [JsonPropertyName("iso")]
    public long? Iso { get; set; }
  }

  public class Links
  {
    [JsonPropertyName("self")]
    public Uri? Self { get; set; }

    [JsonPropertyName("html")]
    public Uri? Html { get; set; }

    [JsonPropertyName("download")]
    public Uri? Download { get; set; }

    [JsonPropertyName("download_location")]
    public Uri? DownloadLocation { get; set; }
  }

  public class Location
  {
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("city")]
    public string? City { get; set; }

    [JsonPropertyName("country")]
    public string? Country { get; set; }

    [JsonPropertyName("position")]
    public Position? Position { get; set; }
  }

  public class Position
  {
    [JsonPropertyName("latitude")]
    public double? Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public double? Longitude { get; set; }
  }

  public class Urls
  {
    [JsonPropertyName("raw")]
    public Uri? Raw { get; set; }

    [JsonPropertyName("full")]
    public Uri? Full { get; set; }

    [JsonPropertyName("regular")]
    public Uri? Regular { get; set; }

    [JsonPropertyName("small")]
    public Uri? Small { get; set; }

    [JsonPropertyName("thumb")]
    public Uri? Thumb { get; set; }
  }

  public class UnsplashUser
  {
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTimeOffset? UpdatedAt { get; set; }

    [JsonPropertyName("username")]
    public string? Username { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("portfolio_url")]
    public Uri? PortfolioUrl { get; set; }

    [JsonPropertyName("bio")]
    public string? Bio { get; set; }

    [JsonPropertyName("location")]
    public string? Location { get; set; }

    [JsonPropertyName("total_likes")]
    public long? TotalLikes { get; set; }

    [JsonPropertyName("total_photos")]
    public long? TotalPhotos { get; set; }

    [JsonPropertyName("total_collections")]
    public long? TotalCollections { get; set; }

    [JsonPropertyName("instagram_username")]
    public string? InstagramUsername { get; set; }

    [JsonPropertyName("twitter_username")]
    public string? TwitterUsername { get; set; }

    [JsonPropertyName("links")]
    public UserLinks? Links { get; set; }
  }

  public class UserLinks
  {
    [JsonPropertyName("self")]
    public Uri? Self { get; set; }

    [JsonPropertyName("html")]
    public Uri? Html { get; set; }

    [JsonPropertyName("photos")]
    public Uri? Photos { get; set; }

    [JsonPropertyName("likes")]
    public Uri? Likes { get; set; }

    [JsonPropertyName("portfolio")]
    public Uri? Portfolio { get; set; }
  }
}
