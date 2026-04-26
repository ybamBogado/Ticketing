import './Loader.css';

export default function Loader() {
    return (
        <div className="loader-container">
            <div className="logo-wrapper">
                
                <div className="spinner-border text-primary custom-spinner" role="status"></div>
                
                <img 
                    src="https://copilot.microsoft.com/th/id/BCO.6bd8487f-b1cf-4e6a-846c-de1acd93d166.png" 
                    alt="Logo" 
                    className="loader-logo" 
                />
            </div>
        </div>
    );
}
