object Form1: TForm1
  Left = 544
  Top = 324
  Width = 1160
  Height = 667
  Caption = 'Form1'
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  OnClose = FormClose
  OnCreate = FormCreate
  PixelsPerInch = 96
  TextHeight = 13
  object Edit1: TEdit
    Left = 120
    Top = 80
    Width = 345
    Height = 21
    TabOrder = 0
    Text = 'Edit1'
  end
  object Button1: TButton
    Left = 328
    Top = 208
    Width = 169
    Height = 49
    Caption = 'start'
    TabOrder = 1
    OnClick = Button1Click
  end
  object Button2: TButton
    Left = 520
    Top = 192
    Width = 129
    Height = 57
    Caption = 'stop'
    TabOrder = 2
    OnClick = Button2Click
  end
  object MsTimer1: TMsTimer
    Enabled = False
    OnTimer = MsTimer1Timer
    Left = 552
    Top = 72
  end
  object ClientSocket1: TClientSocket
    Active = False
    ClientType = ctNonBlocking
    Port = 0
    OnConnecting = ClientSocket1Connecting
    OnConnect = ClientSocket1Connect
    Left = 160
    Top = 184
  end
end
