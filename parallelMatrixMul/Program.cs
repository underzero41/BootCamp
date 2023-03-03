const int N = 1000;
const int THREADNUMBERS = 10;

int[,] serialMulRes = new int[N,N]; //результат умножения матриц в однопотоке
int[,] threadMulRes = new int[N,N]; //результат параллельного умножения матриц

int[,] MatrixGenerator(int rows, int columns)
{
    Random _rand = new Random();
    int[,] res = new int[rows, columns];
    for (int i = 0; i < res.GetLength(0); i++)
    {
        for (int j = 0; j < res.GetLength(1); j++)
        {
            res[i,j] = _rand.Next(-100, 100);
        }
    }
    return res;
}

int[,] firstMatrix = MatrixGenerator(N,N);
int[,] secondMatrix = MatrixGenerator(N,N);

void SerialMatrixMul(int[,] a, int[,] b)
{
    if(a.GetLength(1) != b.GetLength(0)) throw new Exception("Couldnt multiplicate those matrix");
    for (int i = 0; i < a.GetLength(0); i++)
    {
        for (int j = 0; j < b.GetLength(1); j++)
        {
            for (int k = 0; k < b.GetLength(0); k++)
            {
                serialMulRes[i,j] += a[i,k] * b[k,j];
            }
        }
    }
}

SerialMatrixMul(firstMatrix, secondMatrix);

void PrepareParallelMatrixMul(int[,] a, int[,] b)
{
    if(a.GetLength(1) != b.GetLength(0)) throw new Exception("Couldnt multiplicate those matrix");
    int eachThreadCalc = N/THREADNUMBERS;
    var threadsList = new List<Thread>();
    for (int i = 0; i < THREADNUMBERS; i++)
    {
        int startPos = i * eachThreadCalc;
        int endPos = (i+1) * eachThreadCalc;
        //если последний поток
        if(i == THREADNUMBERS -1) endPos = N;
        threadsList.Add(new Thread(() => ParallelMatrixMul(a, b, startPos, endPos)));
        threadsList[i].Start();
    }
    for (int i = 0; i < THREADNUMBERS; i++)
    {
        threadsList[i].Join();
    }
}

void ParallelMatrixMul(int[,] a, int[,] b, int startPos, int endPos)
{
    for (int i = startPos; i < endPos; i++)
    {
        for (int j = 0; j < b.GetLength(1); j++)
        {
            for (int k = 0; k < b.GetLength(0); k++)
            {
                threadMulRes[i,j] += a[i,k] * b[k,j];
            }
        }
    }
}

bool EqualityMatrix(int[,] fmatrix, int[,] smatrix)
{
    bool res = true;
    for (int i = 0; i < fmatrix.GetLength(0); i++)
    {
        for (int j = 0; j < smatrix.GetLength(1); j++)
        {
            res = res && (fmatrix[i,j] == smatrix[i,j]);
        }
    }

    return res;
}

PrepareParallelMatrixMul(firstMatrix, secondMatrix);

Console.WriteLine(EqualityMatrix(firstMatrix, secondMatrix));