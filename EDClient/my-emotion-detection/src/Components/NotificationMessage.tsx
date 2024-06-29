import React, { useEffect, useRef } from 'react';

type NotificationMessageProps = {
  message: string;
  onClose: () => void;
  type: 'error' | 'success';
};

export const NotificationMessage: React.FC<NotificationMessageProps> = ({ message, onClose, type }) => {
  const timerRef = useRef<any | null>(null);

  useEffect(() => {
    // Set a timer to automatically close the message after 5 seconds
    timerRef.current = setTimeout(onClose, 2000);

    // Add an event listener for the escape key
    const handleKeyDown = (event: KeyboardEvent) => {
      if (event.key === 'Escape') {
        onClose();
      }
    };
    window.addEventListener('keydown', handleKeyDown);

    // Cleanup function to clear the timer and remove the event listener
    return () => {
      if (timerRef.current) {
        clearTimeout(timerRef.current);
      }
      window.removeEventListener('keydown', handleKeyDown);
    };
  }, [onClose]);

  const isError = type === 'error';

  const styles = {
    container: {
      position: 'fixed' as const,
      top: '5%', // Adjust this value to control vertical positioning
      left: '50%',
      transform: 'translateX(-50%)',
      width: '80%', // Adjust this value to control the width of the message container
      display: 'flex',
      justifyContent: 'center',
      zIndex: 1000, // Ensure the message is on top of other content
    },
    message: {
      fontFamily: "system-ui, -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu, Cantarell, 'Open Sans', 'Helvetica Neue', sans-serif",
      width: '320px',
      padding: '12px',
      display: 'flex',
      flexDirection: 'row' as const,
      alignItems: 'center',
      justifyContent: 'start',
      backgroundColor: isError ? '#EF665B' : '#EDFBD8',
      borderRadius: '8px',
      border: isError ? 'none' : '1px solid #84D65A',
      boxShadow: '0px 0px 5px -3px #111',
    },
    icon: {
      width: '20px',
      height: '20px',
      transform: 'translateY(-2px)',
      marginRight: '8px',
    },
    iconPath: {
      fill: isError ? '#fff' : '#84D65A',
    },
    title: {
      fontWeight: 500,
      fontSize: '14px',
      color: isError ? '#fff' : '#2B641E',
    },
    close: {
      width: '20px',
      height: '20px',
      cursor: 'pointer',
      marginLeft: 'auto',
    },
    closePath: {
      fill: isError ? '#fff' : '#2B641E',
    },
  };

  return (
    <div style={styles.container}>
      <div style={styles.message}>
        <svg style={styles.icon} viewBox="0 0 24 24">
          <path
            style={styles.iconPath}
            d="M12 2C6.477 2 2 6.477 2 12s4.477 10 10 10 10-4.477 10-10S17.523 2 12 2zm0 18c-4.411 0-8-3.589-8-8s3.589-8 8-8 8 3.589 8 8-3.589 8-8 8zm-1-7h2v2h-2zm0-6h2v4h-2z"
          />
        </svg>
        <div style={styles.title}>{message}</div>
        <svg style={styles.close} viewBox="0 0 24 24" onClick={onClose}>
          <path
            style={styles.closePath}
            d="M18.364 5.636l-1.414-1.414L12 9.172 7.05 4.222 5.636 5.636 10.586 10.586 5.636 15.536l1.414 1.414L12 12.828l4.95 4.95 1.414-1.414-4.95-4.95 4.95-4.95z"
          />
        </svg>
      </div>
    </div>
  );
};
