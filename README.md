# Cinemachine中的通用震屏方案  
## 简介
&emsp;&emsp;ExteninShakeScene和PipelineNoiseShakeSceneshi是练习Component和Extension写法的临时方案展示。  
&emsp;&emsp;ScreenShakeScene是通用震屏方案展示。  
## 效果：
![](show.gif)  
## 使用
&emsp;&emsp;通用震屏方案是对Impulse系统进行简化和特殊化处理后的震屏方案。  
&emsp;&emsp;使用时继承ISignalSource6D实现所需要的噪音类，即可快速实现项目所需的震屏方案。 
## 其他：
&emsp;&emsp;导入前记得先通过Package Manager安装Cinemachine。  