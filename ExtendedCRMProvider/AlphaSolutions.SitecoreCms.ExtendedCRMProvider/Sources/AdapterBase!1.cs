namespace AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Sources
{
    internal class AdapterBase<T>
    {
        private readonly T adaptee;

        protected AdapterBase(T adaptee)
        {
            this.adaptee = adaptee;
        }

        internal T Adaptee
        {
            get
            {
                return this.adaptee;
            }
        }
    }
}

