# Day 06 – Bootcamp .NET
### Multithreading

# Contents
1. [Chapter I](#chapter-i) \
	[General Rules](#general-rules)
2. [Chapter II](#chapter-ii) \
	[Rules of the Day](#rules-of-the-day)
3. [Chapter III](#chapter-iii) \
	[Intro](#intro)
4. [Chapter IV](#chapter-iv) \
	[In addition](#in-addition)
5. [Chapter V](#chapter-v) \
  [Exercise 00 – The laws of the world](#exercise-00-the-laws-of-the-world) 
6. [Chapter VI](#chapter-vi) \
  [Exercise 01 – A race for resources](#exercise-01-a-race-for-resources)
7. [Chapter VII](#chapter-vii) \
  [Exercise 02 – Parallels](#exercise-02-parallels)
8. [Chapter VIII](#chapter-viii) \
  [Exercise 03 – Ready](#exercise-03-ready)
9. [Chapter IX](#chapter-ix) \
  [Exercise 04 – Set, Go!](#exercise-04-set-go)
10. [Chapter X](#chapter-x) \
  [Bonus A little bit random](#bonus-a-little-bit-random)

# Chapter I

## General Rules
- Make sure you have [the .NET 5 SDK](<https://dotnet.microsoft.com/download>) installed on your computer and use it.
- Remember, your code will be read! Pay special attention to the design of your code and the naming of variables. Adhere to commonly accepted [C# Coding Conventions](<https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions>).
- Choose your own IDE that is convenient for you.
- The program must be able to run from the dotnet command line.
- Each of the exercise contains examples of input and output. The solution should use them as the correct format.
- At the beginning of each task, there is a list of allowed language constructs.
- If you find the problem difficult to solve, ask questions to other piscine participants, the Internet, Google or go to StackOverflow.
- You may see the main features of C# language in [official specification](<https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/introduction>).
- Avoid **hard coding** and **"magic numbers"**.
- You demonstrate the complete solution, the correct result of the program is just one of the ways to check its correct operation. Therefore, when it is necessary to obtain a certain output as a result of the work of your programs, it is forbidden to show a pre-calculated result.
- Pay special attention to the terms highlighted in **bold** font: their study will be useful to you both in performing the current task, and in your future career of a .NET developer.
- Have fun :)


# Chapter II
##  Rules of the Day

- All projects must be in the same solution.
- Use a console application created based on a standard .NET SDK template.
- Use **var**.
- The name of the solution (and its separate catalog) is d_{xx}, where xx are the digits of the current day.
- To format the output data, use the en-GB **culture**: N2 for the output of monetary amounts, d for dates.

## What's new

- TimeSpan
- Parallel computing, multithreading
- Deadlocks, race conditions
- PLINQ


# Chapter III
## Intro

Times, people and tasks change, but there are things in the world that remain eternal. Love, the theory of relativity, the sunshine of the spotless mind. Queues. Why don't we go back to the question that we already considered at the start of our course?

During Day 01, we implemented a subject model based on the work of the store, its customers and cash registers. Let's reuse this model, but finish the queue emulation.

Use your own result of Day 01 or download the source code of the [domain model](<https://github.com/smarthead/dotnet_exercise06>). This will be the basis of our project, study it and figure it out if you have not encountered the exercises of Day 01 yet. 

Now let’s add some complexity and parallel work. Our today’s objective is to create a full-fledged simulation of the parallel operation of several cash registers with queues in the store.


# Chapter IV
## In addition

Multithreaded programming in .NET is a very deep topic, which is simply impossible to study fully in one day. Today you will only get a superficial understanding of some of its possibilities. 

However, if you wish, you can get acquainted with this area of knowledge with the help of this free online book: [Threading in C#](<http://www.albahari.com/threading/>).


# Chapter V
## Exercise 00 – The laws of the world

“The story so far:
In the beginning the Universe was created.
This has made a lot of people very angry and been widely regarded as a bad move.”

**― Douglas Adams, The Restaurant at the End of the Universe**

Cash registers do not cope with customers and their purchases instantly. Add two new properties to the *CashRegister* class: the time that the cashier spends on each item in the basket, and the time that the cashier spends switching between customers. For simplicity, let it be the number of seconds. Decide for yourself whether you want to use the **TimeSpan type**.

Let these parameters be defined globally for the store: implement this in the constructors of the *CashRegister* and *Store* classes. Modify the application so that the values of these parameters (in seconds) are taken at startup from the application configuration in the **appsettings.json** file. It is assumed that all the cash registers of the store work and charge in [parallel](<https://docs.microsoft.com/en-us/dotnet/standard/threading/threads-and-threading>), don’t they? Each in its own flow. 

Let's implement the emulation of one cash register’s busyness: add the *Process* method to the *CashRegister*, within which one customer will be processed. In the method, [pause the current processing flow](<https://docs.microsoft.com/en-us/dotnet/standard/threading/pausing-and-resuming-threads>) for the time corresponding to the processing time of one customer and all his goods. Don't forget to count this time!

Add a property to the *CashRegister* class that will let you get the entire time of loading the cash register. Make this time initially zero, but increase every time a new user is processed - increasing by the time the total processing took. 
We have emulated the cash register load. Try debugging it and see if the delays are performed correctly.


# Chapter VI
## Exercise 01 – A race for resources

“Profitable bookstores sell books. Unprofitable book sellers store books.”

**― Mokokoma Mokhonoana**

In our model, each customer takes goods from the same storage - the _Storage_ class is responsible for its work. And if there can be many cash registers, the store's storage is still one. Which means parallel cash registers claim a limited number of goods in one storage - thus creating competition. 

When using multiple threads, it is important to understand that they can block shared resources. Moreover, this block is important for keeping the data **consistent**. For a deeper understanding, refer to the [best practices](<https://docs.microsoft.com/en-us/dotnet/standard/threading/managed-threading-best-practices>).

Let's add a method to the *Storage* class that allows you to take the required number of goods from the storage. Use the tools of the [Interlocked](<https://docs.microsoft.com/en-us/dotnet/api/system.threading.interlocked?view=net-5.0>) class to provide **a thread-safe mechanism for locking** resources.

# Chapter VII
## Exercise 02 – Parallels

“I am free of all prejudice. I hate everyone equally. ”

**― W.C. Fields**

Now that we have implemented the operation of each individual cash register and the mechanism for "buying" goods from the storage, let's make sure that the store runs all the cash registers in parallel.

Add the *OpenRegisters* method to the *Store* class, which [will run a separate thread](<https://docs.microsoft.com/en-us/dotnet/standard/threading/creating-threads-and-passing-data-at-start-time>) for all the store's cash registers: as long as there are goods in the storage, each cash register must consistently process each customer in its queue. In this case, the "purchase" should be made with a decrease in the corresponding number of goods in the storage. 

Create a store with several cash registers and manually add several customers with goods to their queues. Check yourself by debugging, is everything working correctly? Output information about each customer at the checkout to the console.


# Chapter VIII
## Exercise 03 – Ready

“It ain't no fun if the homies can't have none. ”

**― Snoop Dogg**

Now let's turn to the final part of our equation: customers. They also make the choice of the cash register in parallel, let's implement this. When the store starts its work, let it already have some initial number of customers.

Let's say we have 20 customers at the start. Create them and make it so that before the store opens its cash registers, each of these customers will fill their cart and stand in line at the checkout, but do it in parallel.

[PLINQ](<https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/introduction-to-plinq>) tools will help you here, look towards the **Parallel.Foreach** method. Keep in mind that you would have to change the type of a *Customer* queue in the *CashRegister* class to **ConcurrentQueue**. Why is that?

We have provided our store with starting customers and their parallel queue selection. Try debugging your code and see if everything is running correctly.


# Chapter IX
## Exercise 04 – Set, Go!

“Any fool can know. The point is to understand.”

**― Albert Einstein**

It seems that all the pieces of the puzzle are implemented separately, so it's time to put them together. Add the application code to emulate the following situation. 

There is a store where 4 cash registers work at the same time and its storage holds up to 50 goods. Each customer may have from 1 to 7 items in the shopping cart. It takes 2 seconds to charge one item. It takes 4 seconds to change the customer. While the store is open, a new customer appears every 7 seconds.

When cash registers start working, there are 10 customers already in the store. You need to simulate two modes of the store operation.

Customers always choose the shortest queue. New customers always select the queue with the fewest customers, assuming that the queue is moving faster.

Customers choose the queue with the least number of goods.  New customers always select the queue with the least number of goods in their baskets, making the assumption that the queue where customers have fewer baskets moves faster.

For each customer who has completed the purchase, output to the console: the cash register, the customer, the number of goods in the cart, the number of customers in the queue behind them. After the store closes, output for each cash register: the cash register, the average time of the customer in the queue at this checkout (for this you have the total processing time of all and the number of past customers).


# Chapter X
## Bonus A little bit random

“Ph'nglui mglw'nafh Cthulhu R'lyeh wgah'nagl fhtagn.

**― H.P. Lovecraft, The Call of Cthulhu**

We will bring even more chaos to the work of our emulation, making it close to reality. 

Change the application code so that the processing time of one customer and one item at each cash register is not a constant value, global for the entire store, but is calculated as a random number of seconds from 1 to the specified maximum (previously used and defined in appsettings.json parameters timePerItem and timePerCustomer). For each cash register. 

For each customer who has completed the purchase, output to the console: the cash register, the customer, the number of goods in the shopping cart, the number of customers in the queue behind them. After the store closes, output for each cash register: the cash register, the average time of the customer in the queue at this checkout (for this you have the total processing time of all and the number of past customers).
