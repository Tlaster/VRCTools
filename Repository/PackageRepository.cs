using CommunityToolkit.Mvvm.DependencyInjection;
using Realms;

namespace VRChatCreatorTools.Repository;

internal class PackageRepository
{
    private readonly Realm _realm = Ioc.Default.GetRequiredService<Realm>();
    
}