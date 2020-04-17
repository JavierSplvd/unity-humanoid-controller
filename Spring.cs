public class Spring
{
    private float a, v, x, x0, k, m = 0f;

    public Spring(float k, float m, float x0)
    {
        this.k = k;
        this.m = m;
        this.x0 = x0;
        this.x = x0;
    }

    public void Update(float deltaTime)
    {
        a = - k * (x-x0) / m;
        v = a * deltaTime + 0.8f * v; // 20% damping
        x += v * deltaTime;
    }

    public float GetX()
    {
        return x;
    }

    public float GetX0()
    {
        return x0;
    }

    public void SetX0(float x0)
    {
        this.x0 = x0;
    }
}