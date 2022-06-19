using lib.image;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRFZ_Auto.lib.image
{
    public class CIE_DE_2000
    {
		/**
		 * 
		 * 0-0.25△E非；常小或没有；理想匹配

0.25-0.5△E；微小；可接受的匹配

0.5-1.0△E；微小到中等；在一些应用中可接受

1.0-2.0△E：中等；在特定应用中可接受

2.0-4.0△E：有差距；在特定应用中可接受

4.0△E以上：非常大；在大部分应用中不可接受
		 */

		static double _25_7 = Math.Pow(25, 7);
		public static double delta_CIE2000(double L1, double a1, double b1, double L2, double a2, double b2)
		{ 
			var Cab1 = Math.Sqrt(a1 * a1 + b1 * b1);
			var Cab2 = Math.Sqrt(a2 * a2 + b2* b2);
			//var D_Cab=
			var Cabavg = (Cab1 + Cab2) / 2;
			var Cab1_7 = Math.Pow(Cab1, 7);
			var Cab2_7 = Math.Pow(Cab2, 7);
			var G = 0.5 * (1 - Math.Sqrt(Cabavg / (Cabavg + _25_7))); 
			var L1_ = L1;
			var L2_ = L2;
			var Lavg = (L1 + L2) / 2;
			var DL = L1_ - L2_;
			var a1_ = (1 + G) * a1;
			var a2_ = (1 + G) * a2;
			var b1_ = b1;
			var b2_ = b2;
			var Cab1_ = Math.Sqrt(a1_ * a1_ + b1_ * b1_);
			var Cab2_ = Math.Sqrt(a2_ * a2_ + b2_ * b2_);
			var hab1_ = Math.Atan2(b1_,a1_)%360;
			var hab2_ = Math.Atan2(b2_, a2_) % 360; 
			var DCab = Cab2_ - Cab1_;
			var Cab_avg = (Cab1_ + Cab2_) / 2;
			var Dh_abs =Math.Abs( hab1_ - hab2_);
			var dh = 0d;
			
			var Havg = 0d;
			if (Dh_abs<=180)
            {
				dh = hab2_ - hab1_;
				Havg = (hab2_ + hab1_) / 2;

			}
			else
            {

				Havg = ((hab2_ + hab1_+360) / 2) ;
				if (hab2_<= hab1_)
                {
					dh = hab2_ - hab1_+360;
				}
				else
                {
					dh = hab2_ - hab1_-360;
				}
            }
			 
			var DHab_ = 2 * Math.Sqrt(Cab1_ * Cab2_) * Math.Sin((dh) /2);
			var T = 1 - 0.17 * Math.Cos(Havg - 30) + 0.24 * Math.Cos(Havg * 2)
				+ 0.32 * Math.Cos(3 * Havg - 6) - 0.20 * Math.Cos(4 * Havg - 63);
			var SL = 1 + 0.015 * (Lavg - 50) * (Lavg - 50) / Math.Sqrt(20+ (Lavg - 50) * (Lavg - 50));
			var SC = 1 + 0.045 * Cab_avg;
			var SH = 1 + 0.015 * T * Cab_avg;
			var _7 = Math.Pow(Cab_avg, 7);
			var floor= Math.Floor((Havg - 275) / 25);
			var RT = -2 * Math.Sqrt(_7 / (_7 + _25_7)) * Math.Sin(60*Math.Exp(-floor* floor));
			var KL=1;
			var KC = 1;
			var KH = 1;
			var DE =
				Math.Sqrt(
					Math.Pow(DL / (KL * SL), 2) +
					Math.Pow(DCab / (KC * SC), 2) +
				Math.Pow(DHab_ / (KH * SH), 2) +
				RT * DCab / (KC * SC) * DHab_ / (KH * SH)
					) ;
			return DE;
		}

		public static double delta_E00(double L1, double a1, double b1, double L2, double a2, double b2)
		{
			double E00 = 0;               //CIEDE2000色差E00
			double LL1, LL2, aa1, aa2, bb1, bb2; //声明L' a' b' （1,2）
			double delta_LL, delta_CC, delta_hh, delta_HH;        // 第二部的四个量
			double kL, kC, kH;
			double RT = 0;                //旋转函数RT
			double G = 0;                  //G表示CIELab 颜色空间a轴的调整因子,是彩度的函数.
			double mean_Cab = 0;    //两个样品彩度的算术平均值
			double SL, SC, SH, T;
			//------------------------------------------
			kL = 1;
			kC = 1;
			kH = 1;
			//------------------------------------------
			mean_Cab = (CaiDu(a1, b1) + CaiDu(a2, b2)) / 2;
			double mean_Cab_pow7 = Math.Pow(mean_Cab, 7);       //两彩度平均值的7次方
			G = 0.5 * (1 - Math.Pow(mean_Cab_pow7 / (mean_Cab_pow7 + Math.Pow(25, 7)), 0.5));

			LL1 = L1;
			aa1 = a1 * (1 + G);
			bb1 = b1;

			LL2 = L2;
			aa2 = a2 * (1 + G);
			bb2 = b2;

			double CC1, CC2;               //两样本的彩度值
			CC1 = CaiDu(aa1, bb1);
			CC2 = CaiDu(aa2, bb2);
			double hh1, hh2;                  //两样本的色调角
			hh1 = SeDiaoJiao(aa1, bb1);
			hh2 = SeDiaoJiao(aa2, bb2);

			delta_LL = LL1 - LL2;
			delta_CC = CC1 - CC2;
			delta_hh = SeDiaoJiao(aa1, bb1) - SeDiaoJiao(aa2, bb2);
			delta_HH = 2 * Math.Sin(Math.PI * delta_hh / 360) * Math.Pow(CC1 * CC2, 0.5);

			//-------第三步--------------
			//计算公式中的加权函数SL,SC,SH,T
			double mean_LL = (LL1 + LL2) / 2;
			double mean_CC = (CC1 + CC2) / 2;
			double mean_hh = (hh1 + hh2) / 2;

			SL = 1 + 0.015 * Math.Pow(mean_LL - 50, 2) / Math.Pow(20 + Math.Pow(mean_LL - 50, 2), 0.5);
			SC = 1 + 0.045 * mean_CC;
			T = 1 - 0.17 * Math.Cos((mean_hh - 30) * Math.PI / 180) + 0.24 * Math.Cos((2 * mean_hh) * Math.PI / 180)
				+ 0.32 * Math.Cos((3 * mean_hh + 6) * Math.PI / 180) - 0.2 * Math.Cos((4 * mean_hh - 63) * Math.PI / 180);
			SH = 1 + 0.015 * mean_CC * T;

			//------第四步--------
			//计算公式中的RT
			double mean_CC_pow7 = Math.Pow(mean_CC, 7);
			double RC = 2 * Math.Pow(mean_CC_pow7 / (mean_CC_pow7 + Math.Pow(25, 7)), 0.5);
			double delta_xita = 30 * Math.Exp(-Math.Pow((mean_hh - 275) / 25, 2));        //△θ 以°为单位
			RT = -Math.Sin((2 * delta_xita) * Math.PI / 180) * RC;

			double L_item, C_item, H_item;
			L_item = delta_LL / (kL * SL);
			C_item = delta_CC / (kC * SC);
			H_item = delta_HH / (kH * SH);

			E00 = Math.Pow(L_item * L_item + C_item * C_item + H_item * H_item + RT * C_item * H_item, 0.5);

			return E00;
		}
		//彩度计算
		static double CaiDu(double a, double b)
		{
			double Cab = 0;
			Cab = Math.Pow(a * a + b * b, 0.5);
			return Cab;
		}
		//色调角计算
		static double SeDiaoJiao(double a, double b)
		{
			double h = 0;
			double hab = 0;
			if (a == 0) return 90;
			h = (180 / Math.PI) * Math.Atan(b / a);           //有正有负

			if (a > 0 && b > 0)
			{
				hab = h;
			}
			else if (a < 0 && b > 0)
			{
				hab = 180 + h;
			}
			else if (a < 0 && b < 0)
			{
				hab = 180 + h;
			}
			else     //a>0&&b<0
			{
				hab = 360 + h;
			}
			return hab;
		}
		public static double Delta(Color c1,Color c2)
		{
			var LAB1=RGB2LAB(c1.R, c1.G, c1.B);
			var LAB2=RGB2LAB(c2.R, c2.G, c2.B);
			return delta_CIE2000(LAB1.L,LAB1.a,LAB1.b,
				LAB2.L,LAB2.a,LAB2.b); 
		}
		public static double Delta(ImageColor c1, ImageColor c2)
		{
			var LAB1 = RGB2LAB(c1.R, c1.G, c1.B);
			var LAB2 = RGB2LAB(c2.R, c2.G, c2.B);
			return delta_CIE2000(LAB1.L, LAB1.a, LAB1.b,
				LAB2.L, LAB2.a, LAB2.b); 
		}
		public static double Delta(LAB LAB1, LAB LAB2)
		{ 
			var D1= delta_CIE2000(LAB1.L, LAB1.a, LAB1.b,
				LAB2.L, LAB2.a, LAB2.b); 
			var D2= delta_E00(LAB1.L, LAB1.a, LAB1.b,
				LAB2.L, LAB2.a, LAB2.b);
			return D1;
		}
		public static LAB RGB2LAB(int r, int g, int b)
		{
			double R = r / 255d;
			double G = g / 255d;
			double B = b / 255d;
			{
				var t = R;
				if (t > 0.04045)
				{
					t = Math.Pow((t + 0.055) / 1.055, 2.4);

				}
				else
				{
					t = t / 12.92;
				}
				R = t;
			}
			{
				var t = G;
				if (t > 0.04045)
				{
					t = Math.Pow((t + 0.055) / 1.055, 2.4);

				}
				else
				{
					t = t / 12.92;
				}
				G = t;
			}
			{
				var t = B;
				if (t > 0.04045)
				{
					t = Math.Pow((t + 0.055) / 1.055, 2.4);

				}
				else
				{
					t = t / 12.92;
				}
				B = t;
			}
			//D50  
		/*	var X = 0.436052025 * R + 0.385081593 * G + 0.143087414 * B;
			var Y = 0.222491598 * R + 0.716886060 * G + 0.060621486 * B;
			var Z = 0.013929122 * R + 0.097097002 * G + 0.714185470 * B;
			*/
			//D65
		 	var X = 0.412453 * R + 0.357580 * G + 0.180423 * B;
			var Y = 0.212671 * R + 0.715160 * G + 0.072169 * B;
			var Z = 0.019334 * R + 0.119193 * G + 0.950227 * B;
		 
			//D65
			// 
			//wp = [0.964296 1.0 0.825106];
			//D50
		 //	var xyz_ref_white = new double[] { 0.964296, 1.0, 0.825106 };
		//D65
		  	var xyz_ref_white = new double[] { 0.950456, 1.0, 1.088754 };  
			var x = X / xyz_ref_white[0];
			var y = Y / xyz_ref_white[1];
			var z =Z / xyz_ref_white[2];
            {
				var t = x;
				if(t> xyz_t0)
                {
					t = Math.Pow(t, DIV_1_3);
                }else
                {
					t = xyz_t1 * t + xyz_t2;

				}
				x = t;
            }
			{
				var t = y;
				if (t > xyz_t0)
				{
					t = Math.Pow(t, DIV_1_3);
				}
				else
				{
					t = xyz_t1 * t + xyz_t2;

				}
				y= t;
			}
			{
				var t = z;
				if (t > xyz_t0)
				{
					t = Math.Pow(t, DIV_1_3);
				}
				else
				{
					t = xyz_t1 * t + xyz_t2;

				}
				z = t;
			} 
			return new LAB(116 * y - 16, 500 * (x - y), 200 * (y - z));
		}
		static double xyz_t0= 6d / 29d* 6d / 29d* 6d / 29d;
		static double DIV_1_3 = 1d/3d;
		static double xyz_t1 = (1d / 3d) * (29d / 6d) * (29d / 6d);
		static double xyz_t2 = 16d/116d;
		public struct LAB
        {
			public double L;
			public double a;
			public double b;
			public LAB(double l,double a,double b)
            {
				
				L = l;
				this.a = a;
				this.b = b;
            }
		}
    }
}

 
 

class ColorDifferences
{ 
	public static double delta_E00(double L1, double a1, double b1, double L2, double a2, double b2)
	{
		double E00 = 0;               //CIEDE2000色差E00
		double LL1, LL2, aa1, aa2, bb1, bb2; //声明L' a' b' （1,2）
		double delta_LL, delta_CC, delta_hh, delta_HH;        // 第二部的四个量
		double kL, kC, kH;
		double RT = 0;                //旋转函数RT
		double G = 0;                  //G表示CIELab 颜色空间a轴的调整因子,是彩度的函数.
		double mean_Cab = 0;    //两个样品彩度的算术平均值
		double SL, SC, SH, T;
		//------------------------------------------
		kL = 1;
		kC = 1;
		kH = 1;
		//------------------------------------------
		mean_Cab = (CaiDu(a1, b1) + CaiDu(a2, b2)) / 2;
		double mean_Cab_pow7 = Math.Pow(mean_Cab, 7);       //两彩度平均值的7次方
		G = 0.5 * (1 - Math.Pow(mean_Cab_pow7 / (mean_Cab_pow7 + Math.Pow(25, 7)), 0.5));

		LL1 = L1;
		aa1 = a1 * (1 + G);
		bb1 = b1;

		LL2 = L2;
		aa2 = a2 * (1 + G);
		bb2 = b2;

		double CC1, CC2;               //两样本的彩度值
		CC1 = CaiDu(aa1, bb1);
		CC2 = CaiDu(aa2, bb2);
		double hh1, hh2;                  //两样本的色调角
		hh1 = SeDiaoJiao(aa1, bb1);
		hh2 = SeDiaoJiao(aa2, bb2);

		delta_LL = LL1 - LL2;
		delta_CC = CC1 - CC2;
		delta_hh = SeDiaoJiao(aa1, bb1) - SeDiaoJiao(aa2, bb2);
		delta_HH = 2 * Math.Sin(Math.PI * delta_hh / 360) * Math.Pow(CC1 * CC2, 0.5);

		//-------第三步--------------
		//计算公式中的加权函数SL,SC,SH,T
		double mean_LL = (LL1 + LL2) / 2;
		double mean_CC = (CC1 + CC2) / 2;
		double mean_hh = (hh1 + hh2) / 2;

		SL = 1 + 0.015 * Math.Pow(mean_LL - 50, 2) / Math.Pow(20 + Math.Pow(mean_LL - 50, 2), 0.5);
		SC = 1 + 0.045 * mean_CC;
		T = 1 - 0.17 * Math.Cos((mean_hh - 30) * Math.PI / 180) + 0.24 * Math.Cos((2 * mean_hh) * Math.PI / 180)
			+ 0.32 * Math.Cos((3 * mean_hh + 6) * Math.PI / 180) - 0.2 * Math.Cos((4 * mean_hh - 63) * Math.PI / 180);
		SH = 1 + 0.015 * mean_CC * T;

		//------第四步--------
		//计算公式中的RT
		double mean_CC_pow7 = Math.Pow(mean_CC, 7);
		double RC = 2 * Math.Pow(mean_CC_pow7 / (mean_CC_pow7 + Math.Pow(25, 7)), 0.5);
		double delta_xita = 30 * Math.Exp(-Math.Pow((mean_hh - 275) / 25, 2));        //△θ 以°为单位
		RT = -Math.Sin((2 * delta_xita) * Math.PI / 180) * RC;

		double L_item, C_item, H_item;
		L_item = delta_LL / (kL * SL);
		C_item = delta_CC / (kC * SC);
		H_item = delta_HH / (kH * SH);

		E00 = Math.Pow(L_item * L_item + C_item * C_item + H_item * H_item + RT * C_item * H_item, 0.5);

		return E00;
	}
	//彩度计算
	static double CaiDu(double a, double b)
	{
		double Cab = 0;
		Cab = Math.Pow(a * a + b * b, 0.5);
		return Cab;
	}
	//色调角计算
	static double SeDiaoJiao(double a, double b)
	{
		double h = 0;
		double hab = 0;

		h = (180 / Math.PI) *Math.Atan(b / a);           //有正有负

		if (a > 0 && b > 0)
		{
			hab = h;
		}
		else if (a < 0 && b > 0)
		{
			hab = 180 + h;
		}
		else if (a < 0 && b < 0)
		{
			hab = 180 + h;
		}
		else     //a>0&&b<0
		{
			hab = 360 + h;
		}
		return hab;
	}
};

 
