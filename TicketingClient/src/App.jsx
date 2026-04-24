import { useState } from 'react'
import EventCatalog from './views/EventCatalog.jsx'
import './App.css'

function App() {
  const [count, setCount] = useState(0)

  return (
    <>

      <h1>Ticketinator 2000</h1>
      <p>
        Edit <code>src/App.jsx</code> and save to test <code>HMR</code>
      </p>

      <EventCatalog />
      <button
        className="counter"
        onClick={() => setCount((count) => count + 1)}
      >
        Count is {count}
      </button>


    </>
  )
}

export default App
