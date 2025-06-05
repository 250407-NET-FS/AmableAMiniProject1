
import React from 'react';
import { render, screen, act } from '@testing-library/react';
import { AuthProvider, useAuth, authReducer } from '../context/AuthContext';
import { jwtDecode } from 'jwt-decode';

jest.mock('jwt-decode');


describe('authReducer', () => {
  it('should handle LOGIN', () => {
    const action = { type: 'LOGIN', payload: { id: '123', email: 'test@example.com' } };
    const initialState = { user: null, isAuthenticated: false };
    const result = authReducer(initialState, action);
    expect(result).toEqual({ user: action.payload, isAuthenticated: true });
  });

  it('should handle LOGOUT', () => {
    const initialState = { user: { id: '123' }, isAuthenticated: true };
    const result = authReducer(initialState, { type: 'LOGOUT' });
    expect(result).toEqual({ user: null, isAuthenticated: false });
  });
});


describe('AuthProvider behavior', () => {
  beforeEach(() => {
    localStorage.clear();
    jwtDecode.mockReset();
  });

  const MockConsumer = () => {
    const { user, isAuthenticated } = useAuth();
    return (
      <>
        <div data-testid="auth">{isAuthenticated ? 'Yes' : 'No'}</div>
        <div data-testid="user">{user ? user.fullName : 'None'}</div>
      </>
    );
  };

  it('loads user from valid token on mount', async () => {
    localStorage.setItem('jwt', 'fakeToken');
    jwtDecode.mockReturnValue({
      exp: Date.now(),
      "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress": "user@example.com",
      "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier": "123",
      "http://schemas.microsoft.com/ws/2008/06/identity/claims/role": "User",
    });

    await act(async () =>
      render(
        <AuthProvider>
          <MockConsumer />
        </AuthProvider>
      )
    );

    expect(screen.getByTestId('auth').textContent).toBe('Yes');

  });
});
