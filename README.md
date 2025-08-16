# 🖥️ InspectionNet WPF Portfolio

![License](https://img.shields.io/badge/license-MIT-green.svg)  
![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)  
![WPF](https://img.shields.io/badge/WPF-MVVM-orange.svg)  
![Visual Studio](https://img.shields.io/badge/IDE-Visual%20Studio%202022-blueviolet)

---

## 📌 소개
**InspectionNet WPF Portfolio**는 머신 비전 검사 시스템을 위한  
**MVVM 아키텍처 기반 데모 및 테스트용 포트폴리오 프로젝트**입니다.  

---

## 🚀 주요 기능
- Vision Tool(AI/Rule) 연동 (ex: Cognex V-Pro)  
- 코드 예외 로깅 서비스  
- 서비스 형태의 모듈 구조 (카메라, 조명, 비전 알고리즘 등)  
- 부트스트레퍼를 통한 모듈 연결 및 관리  
- MVVM 아키텍처 적용으로 확장성과 유지보수성 강화  

---

## 🗂️ 구성 요소
InspectionNet.Wpf.Portfolio/

├── InspectionNet.CameraComponent.TestModule/ # 카메라 테스트 모듈
├── InspectionNet.Core/ # 공통 유틸/인터페이스
├── InspectionNet.EnvironmentTools.Logger/ # 로깅 서비스 모듈
├── InspectionNet.LightComponent.TestModule/ # 조명 테스트 모듈
├── InspectionNet.MotionComponent.TestModule/ # 축 제어 테스트 모듈
├── InspectionNet.VisionTool.CognexModule.Common # Cognex 모듈 공통 (WinForms 기반)
├── InspectionNet.VisionTool.TestAiModule/ # AI 테스트 모듈
├── InspectionNet.Winform.Common/ # WinForms 공용 컨트롤
├── InspectionNet.Wpf.Common/ # WPF 공통 유틸
├── InspectionNet.Wpf.MainFrame/ # 메인 UI 및 부트스트레퍼
├── InspectionNet.Wpf.PocProject/ # POC 프로젝트
├── InspectionNet.VisionTool.CognexModule/ # Cognex V-Pro 연동 모듈
└── ReferenceAssemblies/ # 참조 DLL 모음

---

## 🔗 모듈 관계 다이어그램 (개념도)

```mermaid
graph TD

    subgraph Core
        A[InspectionNet.Core]
    end

    subgraph Services
        B[Logger] --> A
        C[Camera Test] --> A
        D[Light Test] --> A
        E[Motion Test] --> A
    end

    subgraph Vision
        F[Cognex Common] --> A
        G[Cognex V-Pro Module] --> F
        H[Test AI Module] --> A
    end

    subgraph UI
        I[Wpf.Common] --> A
        J[Winform.Common] --> A
        K[MainFrame/Bootstrapper] --> A
        K --> B
        K --> C
        K --> D
        K --> E
        K --> F
        K --> H
    end
--

## 🛠️ 개발 환경
- .NET 8.0  
- WPF (MVVM 아키텍처)  
- Visual Studio 2022  
