import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { BrowserRouter, Routes, Route } from 'react-router-dom'
import { AuthProvider } from './context/AuthContext.jsx'
import './index.css'
import EventCatalog from './views/EventCatalog.jsx'
import EventDetail from './views/EventDetail.jsx'
import Login from './views/Login.jsx'

createRoot(document.getElementById('root')).render(
  <StrictMode>
    <AuthProvider>
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<EventCatalog />} />
          <Route path="/login" element={<Login />} />
          <Route path="/event/:eventId" element={<EventDetail />} />
        </Routes>
      </BrowserRouter>
    </AuthProvider>
  </StrictMode>,
)
