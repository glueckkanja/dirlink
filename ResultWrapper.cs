using System;

namespace DirLink
{
    public abstract class ResultWrapper
    {
        private readonly SearchResultWrapper _result;

        protected ResultWrapper(SearchResultWrapper result)
        {
            _result = result;
        }

        public SearchResultWrapper RawResult
        {
            get { return _result; }
        }
    }
}