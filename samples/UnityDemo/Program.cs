using UnityDemo;
using lizzie;

var context = new UnityContext(new MockUnityScheduler());
var lambda = LambdaCompiler.Compile(context, "main-thread(function() { write('Hello from Unity demo') })");
lambda();
