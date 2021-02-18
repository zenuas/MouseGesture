namespace Win32API
{
    public enum HOOKCODES : int
    {
        HC_ACTION = 0,
        HC_GETNEXT = 1,
        HC_SKIP = 2,
        HC_NOREMOVE = 3,
        HC_NOREM = HC_NOREMOVE,
        HC_SYSMODALON = 4,
        HC_SYSMODALOFF = 5
    }
}
