import React, { ReactNode } from 'react';
interface ProtectedRouteProps {
    children: ReactNode;
    requiredRole?: 'Admin' | 'User';
}
export declare const ProtectedRoute: React.FC<ProtectedRouteProps>;
export {};
//# sourceMappingURL=ProtectedRoute.d.ts.map