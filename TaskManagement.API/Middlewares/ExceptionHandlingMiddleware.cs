//namespace TaskManagement.API.Middlewares
//{
//    public class ExceptionHandlingMiddleware 
//    {
//        private readonly RequestDelegate _next;
//        public ExceptionHandlingMiddleware(RequestDelegate next)
//        {
//            _next = next;   
//        }

//        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
//        {
//            try
//            {
//                await next(context);
//            }
//            catch (Exception ex)
//            {
//                context.Response.Clear();
//                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
//                context.Response.ContentType = "application/json";
//                // Customize the error response based on your requirements
//                await context.Response.WriteAsync("An error occurred while processing the request.");
//            }
//        }
//    }
//}
