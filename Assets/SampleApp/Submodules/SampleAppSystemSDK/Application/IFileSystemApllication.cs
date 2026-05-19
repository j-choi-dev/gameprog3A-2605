namespace SampleAppSystemSDK.Application
{
    public interface IFileSystemApllication
    {
        string GetStringFromBinary( string fileName );
        bool CheckInitialize( string fileName );
        bool GetIsExist( string fileName );
    }
}
