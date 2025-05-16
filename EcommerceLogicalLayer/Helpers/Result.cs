
namespace EcommerceLogicalLayer.Helpers
{
    public class Result
    {

        public Result(bool isSeccuss, Erorr? erorr)
        {
            if ((isSeccuss && erorr != null) || (!isSeccuss && erorr == null))
                new Exception();
            Erorr = erorr!;
            IsSeccuss = isSeccuss;
            IsFialer = !isSeccuss;
        }

        public Erorr Erorr { get; }
        public bool IsSeccuss { get; }
        public bool IsFialer { get; }


        public static Result Seccuss() => new Result(true, null);
        public static Result Fialer(Erorr Erorr) => new Result(false, Erorr);

    }

    public class Result<T> : Result
    {

        public Result(T value, bool isSeccuss, Erorr? erorr) : base(isSeccuss, erorr)
        {
            Value = value;
        }

        private Result(bool isSeccuss, Erorr? erorr) : base(isSeccuss, erorr)
        {

        }

        public T? Value { get; }
        public static Result<T> Seccuss<T>(T value) => new Result<T>(value, true, null);
        public static Result<T> Fialer<T>(Erorr Erorr) => new Result<T>(false, Erorr);


    }
}
