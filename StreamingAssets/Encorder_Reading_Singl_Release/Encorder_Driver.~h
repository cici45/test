//---------------------------------------------------------------------------

#ifndef Encorder_DriverH
#define Encorder_DriverH
//---------------------------------------------------------------------------

#include <ser.h>


#include <msxmldom.hpp>
#include <XMLDoc.hpp>
#include <xmldom.hpp>
#include <XMLIntf.hpp>



class Encoder_Driver {


public:
   Encoder_Driver();

   ~Encoder_Driver();

   double Read_Angle();




private:

   AnsiString Com_Address;
   long int Encorder_Puls;// the total puls counts of one circle
   int Encorder_ID;
   long long int Encorder_Puls_Read;
   long long  int Read_Puls();
   long int Baudrate;

   double Position_Offset;
   int  Position_Dir;
   ser *Com_232;

   BYTE Read_Buff[18] ;
   BYTE Send_Buff[18] ;

   long long int Angle_Old;
   long long int Angle_New;

   int MutiCircle_Select;

   long long int Circle_Counter;

   unsigned int Crc_Count(unsigned char pbuf[],unsigned char num);
   bool First_Start;

};


#endif
