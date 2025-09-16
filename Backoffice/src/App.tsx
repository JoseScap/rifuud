import { BrowserRouter, Route, Routes } from 'react-router-dom'
import './App.css'
import { AuthProvider } from './providers/AuthProvider'
import { HomePage } from './pages/home/home-page'
import { LoginPage } from './pages/login/login-page'
import { DashboardPage } from './pages/dashboard/dashboard-page'

function App() {
  return (
    <>
      <BrowserRouter>
        <AuthProvider>
          <Routes>
            <Route path="/" element={<HomePage />} />
            <Route path="/login" element={<LoginPage />} />
            <Route path="/dashboard" element={<DashboardPage />} />
          </Routes>
        </AuthProvider>
      </BrowserRouter>
    </>
  )
}

export default App
