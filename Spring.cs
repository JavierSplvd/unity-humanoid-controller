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

    public void FixedUpdate(float delta)
    {
        a = - k * (x-x0) / m;
        v = a * delta + v - v * 9f * delta;
        x += v * delta;
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