using System;

public static class WeiDEBUG
{
    public static int stackIndex = 0;
    public static string CallStackInfo(string s)
    {
        stackIndex++;
        return s + " : " + stackIndex.ToString();
    }
}