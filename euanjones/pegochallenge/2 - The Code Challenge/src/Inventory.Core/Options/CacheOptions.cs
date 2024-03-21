namespace Inventory.Core;

public class CacheOptions
{
	public const string SectionName = "Cache";

	public int SizeLimit { get; set; }
	public TimeSpan ExpirationTime { get; set; }
}