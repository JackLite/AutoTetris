using System.Threading.Tasks;
using EcsCore;

namespace Core.Tutorial
{
    public class TutorialModule : EcsModule
    {
        protected override Task Setup()
        {
            var data = new EcsOneData<TutorialProgressData>();
            data.SetData(new TutorialProgressData { delay = 2 });
            OneDataDict[typeof(TutorialProgressData)] = data;
            return Task.CompletedTask;
        }
    }
}