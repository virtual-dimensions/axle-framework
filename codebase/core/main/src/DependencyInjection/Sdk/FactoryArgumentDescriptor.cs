using Axle.Reflection;
using Axle.Verification;


namespace Axle.DependencyInjection.Sdk
{
    public class FactoryArgumentDescriptor : IFactoryArgumentDescriptor
    {
        public FactoryArgumentDescriptor(IParameter parameter, string dependencyName)
        {
            Member = parameter.VerifyArgument(nameof(parameter)).IsNotNull().Value;
            Info = new DependencyInfo(parameter.Type, dependencyName.VerifyArgument(nameof(dependencyName)).IsNotNull(), parameter.Name);
        }

        public IParameter Member { get; }
        public DependencyInfo Info { get; }
        public bool Optional => Member.IsOptional;
        public object DefaultValue => Member.DefaultValue;
    }
}