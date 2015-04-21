namespace AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Sources
{
    internal abstract class AdapterFactoryBase<TAdaptee, TAdapter>
    {
        protected AdapterFactoryBase()
        {
        }

        public abstract TAdapter Create(TAdaptee adaptee);
    }
}

