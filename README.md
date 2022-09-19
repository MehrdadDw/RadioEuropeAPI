 # Projects


This is a .NET Solution containing a 3 projects:

 -  API Project
 -  API UnitTests Project 
 -  Test Client for the API end points





### How to start project


To build and start both projects simply run the following command:

```bash
docker-compose build
docker-compose up
```
During this proccess the tests (around 25) and build would be done and then by startinf the containers, server would be listening to port number: 8083. Therefore you can navigate to swagger page: http://localhost:8083/swagger or http://127.0.0.1:8083/swagger

and to stop it you can use:
```bash
docker-compose down
```

However, the Test Client Project can be run manually

```bash
dotnet run ./RadioTest --number 1000 --host localhost:8083
```
The number and host flags should be set during run.



### What API does?

Provides 2 HTTP endpoints that accepts base64-encoded JSON of following format

{"input":"some value to be compared"}

```bash
curl -X POST "http://localhost:8083/v1/diff/<ID>/left" -H "accept: */*" -H "Content-Type:
application/custom" -d "\"eyJpbnB1dCI6InRlc3RWYWx1ZSJ9\""
```
Same format as above for right endpoint is true.

By calling the 3rd endpoint and providing the desired ID, diff-ed value would be returned:

```bash
http://localhost:8083/v1/diff
```
The results provides the following info in JSON format

If value of the "input" property of diffed JSONs is equal, returns “inputs were equal”.

If value of the "input" property of diffed JSONs is not of equal size, returns “inputs are of different size”.

If value of the "input" property of diffed JSONs has the same size, perform a simple diff - returns offsets and lengths of the differences.



### Example

```bash
Left=       "ABCDEFG"
Right=      "ABXXEFX"
Difference= "--??--?"
Result=[(2,2),(6,1)]
```
The result means starting from `second` character with the length of `2` we have first difference. the second difference starts at `6th` position and its length is `1`.

### System Design

#### Matching IDs
To calculate the differences, first of all we need a way to wire left and right with each other, for this the project uses `Redis` as data store and benefits from its fast on memory architecture. 

The difference calculation itself depending on the length of strings can be time consuming but since there is no assumption about the length there is not further optimization considered in this solution.

#### on-Demand Calculation
The difference would be calculated on demand, meaning for same `ID` the calculation could happen multiple times, which also can be enhanced by storing the result for next uses that due to the more complicated solution considering rewrites and recalculation in case of overwrite is not implemented.



#### Time Complexity
The time complexity of implemented diff calculation is of order `O(n)` and is fast.


### Tests
#### Unit Tests 
There is around 30 tests covering service, controllers and utilities.
#### Test client
Test client is a console application providing following options:
```bash
--number   --> number of test requests
--host      --> the url on which the API endpoints are exposed
```
To increase the pressure of test either we can run `multiple instances` of the test client or use tools like `jmeter`.
### Suggestions for better solution

#### Asynchronous Calculation

In a real world scenario, the API users could be 
numerous, and the diff request can happen not imediately, in this situation we can facilitate an `asynchronous solution` using message queues such as `RabbitMQ` to make the calculation happen constantly even when there is no demand and by storing the results we can increase the `CPU efficiency` by decreasing the `idle time`. However, this solution needs some assumptions and overwriting should be taken into account.

This way, after getting both left and right values the calculation starts for future use, no matter wether there already is a demand for the result or not.

#### Concurrent Calculation

In case of `very long` strings, we can also exploit the power of concurrent calculations using threads/tasks, such that each thread/task calculates the difference of handful amount of characters.


