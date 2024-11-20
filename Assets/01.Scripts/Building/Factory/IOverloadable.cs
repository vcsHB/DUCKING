

public interface IOverloadable
{
    public float OverloadLevel {get; protected set; } // default : 0
    public void SetOverload(float level)
    {
        OverloadLevel = level;
    }

    public void SetOffOverload()
    {
        OverloadLevel = 0;
    }
}