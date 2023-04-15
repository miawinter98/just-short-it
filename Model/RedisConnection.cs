namespace JustShortIt.Model;

public record RedisConnection(string ConnectionString, string InstanceName = "JustShortIt");

