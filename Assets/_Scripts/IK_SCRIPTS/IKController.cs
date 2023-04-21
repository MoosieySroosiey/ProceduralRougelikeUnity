using System;
using UnityEngine;

public class IKController : MonoBehaviour
{
    float legspeed = 10f;
    float gait = 0f;
    Vector2 movingDirection = new Vector2(0, 0);
    Vector2 facingDirection = new Vector2(0, 0);
    float mo_co = 1f;
    float x = 0;
    float y = 0;

    Vector2 pointDirection(float x1, float x2, float y1, float y2)
    {

        float degree = (float)(Math.Atan2(y2 - y1, x2 - x1) * (180 / Math.PI));
        return new Vector2(Mathf.Cos(degree * Mathf.Deg2Rad), Mathf.Sin(degree * Mathf.Deg2Rad));
    }
    Vector2 pointDirection(float degree)
    {
        return new Vector2(Mathf.Cos(degree * Mathf.Deg2Rad), Mathf.Sin(degree * Mathf.Deg2Rad));
    }
    float pointDistance(float x1, float y1, float x2, float y2)
    {
        return (float)Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));// Euclidean distance formula 

    }


    float lengthDirX(float len, Vector2 dir)// lenght, direction
    {
        dir = dir.normalized * (float)len;
        return dir.x;
    }

    float lengthDirY(float len, Vector2 dir)// lenght, direction
    {
        dir = dir.normalized * (float)len;
        return dir.y;
    }

    void createIK(float lengthCalf, float lengthThigh)
    {
        //

    }

    void drawLeg(float lengthCalf, float lengthThigh, float hipX, float hipY)
    {
        //**********A*********
        //*********/|*********
        //*******b/ |*********
        //*******/  |*********
        //******/   |*********
        //*****C\   |c********
        //*******\  |*********
        //*******a\ |*********
        //*********\|*********
        //**********B*********
        //********************


        //Rule of Cosines will be here soon 
        float cLength, bLength, aLength, ax, ay, ix, iy, kneeMod, Ax, Ay, Bx, By, Cx, Cy, C2x, C2y;
        //cLenght is the distance from hip to foot

        Ax = hipX;
        Ay = hipY;
        bLength = lengthThigh;
        aLength = lengthCalf;


        ix = 0;//temporary 
        iy = 0;//temporary
        Vector2 alpha, beta;

        //mo_co -> motion counter used for the oscillating animation of foot // move somewhere else

        kneeMod = Ax - (Ax + lengthDirX(1, facingDirection)); //Direction the knee will bend for the "3D" knee

        if (legspeed > 0)
            gait = (float)Math.Pow(legspeed * 2, 0.4); //how big the step is (may need tweaking)
                                                       //Stride is not related to movement speed linearly, it uses a exponent of 0.4.


        //Sin-> Horizontal movement, Cos-> Veritcal movement of the "foot"


        ax = x + lengthDirX(Convert.ToSingle((gait) * ((aLength + bLength) / 4) * (Math.Sin(Mathf.Deg2Rad * mo_co)) - ((legspeed * 1.25)) * 2), movingDirection);
        //x=location x

        ay = (float)(y + ((gait) * ((aLength + bLength) / 6) * (-Math.Cos(Mathf.Deg2Rad * (mo_co)) - 1)));
        //y=location y

        ///IK CALCULATION///
        alpha = pointDirection(Ax, Ay, ax, ay); //angle between hip and foot

        cLength = Math.Min(pointDistance(Ax, Ay, ax, ay), (aLength + bLength)); //distance between hip and foot, restricted to total limb length

        Bx = Ax + lengthDirX(cLength, alpha); //foot x position

        By = Ay + lengthDirY(cLength, alpha); //foot y position

        beta = pointDirection((float)(Mathf.Rad2Deg * (Math.Acos(Math.Min(1, Math.Max(-1, (Math.Pow(bLength, 2) + Math.Pow(cLength, 2) - Math.Pow(aLength, 2)) / (2 * (bLength) * cLength))))))); //"Law of Cosines" to get angle of thigh, _c

        Cx = Ax + lengthDirX(bLength, alpha - beta);//knee x position
        Cy = Ay + lengthDirY(bLength, alpha - beta);//knee y position


        C2x = ix + lengthDirX(pointDistance(ix, iy, Cx, Cy) * kneeMod, pointDirection(ix, iy, Cx, Cy));//"3D" knee x position
        C2y = iy + lengthDirY(pointDistance(ix, iy, Cx, Cy) * kneeMod, pointDirection(ix, iy, Cx, Cy));//"3D" knee y position


        ///////////////////////////////////////////////////////////////////////
       //////////////////DRAWING THE SPRITES WILL GO HERE/////////////////////
      ///////////////////////////////////////////////////////////////////////
    }


    void Start()
    {
        createIK(10, 8);//Crete the IK on Start
    }

}
